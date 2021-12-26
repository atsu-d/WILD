using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemSystem
{
    public class InteractController : MonoBehaviour
    {
        public ManagerReference managers;

        private PlayerInput inputManger;
        private InputAction interactInput, dropInput;

        public ItemManager currentInteraction { get; private set; }
        public ContainerManager currentContainer { get; private set;}
        public ItemData activeItem { get; private set; }

        [Header("Interaction Raycast")]
        public float raycastDistance;
        public float raycastSphereRadius;
        public LayerMask interactableLayer;
        public bool isInteracting { get; private set; }
        private float holdTimer = 0f;
        public float holdPercent { get; private set; }

        [SerializeField] private IntVariable activeHotbar;

        [SerializeField] private Camera cam;
        public static Transform camTr;
        [SerializeField] Transform inventoryTr;

        public event Action OnDiscardItem;

        private void Awake()
        {
            managers.SetInteractManager(this);
  
            inputManger = new PlayerInput();

            camTr = cam.transform;
        }

        private void Start()
        {
            holdTimer = 0f;
            isInteracting = false;
        }

        private void Update()
        {
            FindInteractable();
        }

        private void DropInput(InputAction.CallbackContext context)
        {
            if (activeHotbar.value <= 0) return;

            activeItem = managers.InventoryManager.inventory[activeHotbar.value - 1].data;

            #region Drop to Ground
            if (currentContainer == null)
            {
                if (OnDiscardItem != null) OnDiscardItem();

                managers.InventoryManager.Remove(activeItem);
                Vector3 _origin = inventoryTr.position + camTr.forward * 0.8f;
                ItemManager _interactable = Instantiate(activeItem.Prefab, _origin, Quaternion.identity).GetComponent<ItemManager>();
                _interactable.OnInteract(InteractionType.Drop);

                activeHotbar.Set(0);
            }
            #endregion

            #region Add to storage
            if (currentContainer != null && currentContainer.CanAdd(activeItem))
            {
                Debug.Log(activeItem.name + " added to " + currentContainer.name);
                if (OnDiscardItem != null) OnDiscardItem();

                managers.InventoryManager.Remove(activeItem);
                currentContainer.Add(activeItem);

                activeHotbar.Set(0);
            }
            #endregion

        }

        private void HandleInteraction()
        {
            if (currentInteraction == null) return;

            currentInteraction.ChangeInteractUI(false);

            if (interactInput.ReadValue<float>() > 0)
            {
                isInteracting = true;

                if (currentInteraction.HoldInteract && isInteracting)
                {
                    currentInteraction.ChangeInteractUI(true);
                    holdTimer += Time.deltaTime;

                    holdPercent = holdTimer / currentInteraction.HoldDuration;

                    if (holdPercent > 1f)
                    {
                        currentInteraction.OnInteract(InteractionType.Hold);
                        holdTimer = 0f;
                    }
                }
                else
                {
                    currentInteraction.OnInteract(InteractionType.Tap);
                    holdTimer = 0f;
                    holdPercent = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
                holdPercent = 0f;
                isInteracting = false;
            }
        }

        private void FindInteractable()
        {
            Ray _ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit _hit;

            bool _foundInteractable = Physics.SphereCast(_ray, raycastSphereRadius, out _hit, raycastDistance, interactableLayer);

            if (_foundInteractable)
            {
                ItemManager _currentInteraction = _hit.transform.gameObject.GetComponent<ItemManager>();

                if (_currentInteraction != null && currentInteraction != _currentInteraction)
                { currentInteraction = _currentInteraction; }

                if(activeHotbar.value != 0)
                {
                    ContainerManager _currentContainer = _hit.transform.gameObject.GetComponent<ContainerManager>();
                    if (_currentContainer != null && currentContainer != _currentContainer) currentContainer = _currentContainer;
                }
            }
            else ResetInteraction();

            Debug.DrawRay(_ray.origin, _ray.direction * raycastDistance, _foundInteractable ? Color.green : Color.red);

            HandleInteraction();
        }

        public void ResetInteraction()
        {
            if (currentInteraction != null || currentContainer != null)
            {
                currentInteraction = null;
                currentContainer = null;
            }
        }

        private void OnEnable()
        {
            interactInput = inputManger.Player.Interact;
            interactInput.Enable();

            dropInput = inputManger.Inventory.DropItem;
            dropInput.Enable();
            dropInput.performed += DropInput;
        }

        private void OnDisable()
        {
            interactInput.Disable();
            dropInput.Disable();
        }
    }
}