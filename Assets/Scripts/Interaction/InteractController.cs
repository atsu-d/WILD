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

        public InteractionData interactionData;

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
            interactionData.SetInventory(managers.InventoryManager);

            holdTimer = 0f;
            isInteracting = false;
            activeHotbar.value = 0; //fix later so player will hold same item on game open as game end
        }

        private void Update()
        {
            FindInteractable();
        }

        private void DropInput(InputAction.CallbackContext context)
        {
            if (activeHotbar.value <= 0) return;

            #region Drop to Ground
            if (interactionData.worldContainer == null)
            {
                if (OnDiscardItem != null) OnDiscardItem();

                Vector3 _origin = inventoryTr.position + camTr.forward * 0.8f;
                ItemManager _interactable = Instantiate(interactionData.activeItem.Prefab, _origin, Quaternion.identity).GetComponent<ItemManager>();
                _interactable.OnDrop.OnInteract(_interactable.rb, camTr, _interactable.Data);

                activeHotbar.Set(0);
            }
            #endregion

            #region Add to storage
            if (interactionData.worldContainer != null && interactionData.worldContainer.CanAdd(interactionData.activeItem))
            {
                Debug.Log(interactionData.activeItem.name + " added to " + interactionData.worldContainer.name);
                if (OnDiscardItem != null) OnDiscardItem();

                interactionData.heldItem.OnDeposit.OnInteract();

                activeHotbar.Set(0);
            }
            #endregion

        }

        private void HandleInteraction()
        {
            ItemManager _interactable = interactionData.worldInteractable;
            //if (interactionData.worldInteractable == null) return;
            if (_interactable)
                _interactable.ChangeInteractUI(false);

            if (interactInput.ReadValue<float>() > 0)
            {
                isInteracting = true;

                if (_interactable && _interactable.OnPickUp != null  && isInteracting)
                {
                    _interactable.ChangeInteractUI(true);
                    holdTimer += Time.deltaTime;

                    holdPercent = holdTimer / _interactable.OnPickUp.holdDuration;

                    if (holdPercent > 1f)
                    {
                        _interactable.OnPickUp.OnInteract(_interactable.Data, _interactable.gameObject);
                        holdTimer = 0f;
                        holdPercent = 0f;
                    }
                }
                else
                {
                    if (interactionData.heldItem != null)
                    {
                        if (OnDiscardItem != null) OnDiscardItem();

                        Vector3 _origin = inventoryTr.position + camTr.forward * 0.8f;
                        ItemManager _heldItem = Instantiate(interactionData.activeItem.Prefab, _origin, Quaternion.identity).GetComponent<ItemManager>();
                        _heldItem.OnUse.OnInteract(_heldItem.Data, _heldItem.rb);

                        activeHotbar.Set(0);
                    }

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

                if (_currentInteraction != null && interactionData.worldInteractable != _currentInteraction)
                { interactionData.SetInteractable(_currentInteraction); }

                if(activeHotbar.value != 0)
                {
                    ContainerManager _currentContainer = _hit.transform.gameObject.GetComponent<ContainerManager>();
                    if (_currentContainer != null && interactionData.worldContainer != _currentContainer) interactionData.SetContainer(_currentContainer);
                }
            }
            else ResetInteraction();

            Debug.DrawRay(_ray.origin, _ray.direction * raycastDistance, _foundInteractable ? Color.green : Color.red);

            HandleInteraction();
        }

        public void ResetInteraction()
        {
            if (interactionData.worldInteractable != null || interactionData.worldContainer != null)
            {
                interactionData.ClearInteractable();
                interactionData.ClearContainer();
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