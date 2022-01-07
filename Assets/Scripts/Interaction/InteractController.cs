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
        public InteractionManager interactionManager;

        private PlayerInput inputManger;
        private InputAction interactInput, dropInput;

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
            interactionManager.SetInventory(managers.InventoryManager);
            interactionManager.SetInventoryTransform(inventoryTr);

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
            if (interactionManager.worldContainer == null)
            {
                if (interactionManager.activeItem.DropAction == null)
                    return;

                //if (OnDiscardItem != null) 
                //    OnDiscardItem();

                interactionManager.OnInteract(InteractionType.Drop);
                activeHotbar.Set(0);
            }
            #endregion

            #region Add to storage
            if (interactionManager.worldContainer != null && interactionManager.worldContainer.CanAdd(interactionManager.activeItem.Data))
            {
                if (interactionManager.activeItem.DepositAction == null)
                    return;

                if (OnDiscardItem != null)
                    OnDiscardItem();

                Debug.Log(interactionManager.activeItem.name + " added to " + interactionManager.worldContainer.name);

                interactionManager.OnInteract(InteractionType.Deposit);
                activeHotbar.Set(0);
            }
            #endregion

        }

        private void HandleInteraction()
        {
            ItemManager _interactable = interactionManager.worldInteractable;
            //if (interactionData.worldInteractable == null) return;
            if (_interactable)
                _interactable.ChangeInteractUI(false);

            if (interactInput.ReadValue<float>() > 0)
            {
                isInteracting = true;

                if (_interactable && _interactable.PickUpAction != null  && isInteracting)
                {
                    _interactable.ChangeInteractUI(true);
                    holdTimer += Time.deltaTime;

                    holdPercent = holdTimer / _interactable.PickUpAction.holdDuration;

                    if (holdPercent > 1f)
                    {
                        interactionManager.OnInteract(InteractionType.PickUp);
                        holdTimer = 0f;
                        holdPercent = 0f;
                    }
                }
                else
                {
                    if (interactionManager.activeItem != null)
                    {
                        if (interactionManager.activeItem.UseAction == null)
                            return;

                        if (OnDiscardItem != null) OnDiscardItem();

                        interactionManager.OnInteract(InteractionType.Use);
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

                if (_currentInteraction != null && interactionManager.worldInteractable != _currentInteraction)
                { interactionManager.SetInteractable(_currentInteraction); }

                if(activeHotbar.value != 0)
                {
                    ContainerManager _currentContainer = _hit.transform.gameObject.GetComponent<ContainerManager>();
                    if (_currentContainer != null && interactionManager.worldContainer != _currentContainer) interactionManager.SetContainer(_currentContainer);
                }
            }
            else ResetInteraction();

            Debug.DrawRay(_ray.origin, _ray.direction * raycastDistance, _foundInteractable ? Color.green : Color.red);

            HandleInteraction();
        }

        public void ResetInteraction()
        {
            if (interactionManager.worldInteractable != null || interactionManager.worldContainer != null)
            {
                interactionManager.ClearInteractable();
                interactionManager.ClearContainer();
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