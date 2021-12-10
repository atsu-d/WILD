using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemSystem
{
    public class InteractController : MonoBehaviour
    {
        public static InteractController Controller;
        public PlayerInput inputManger;
        private InputAction interactInput;

        [Header("Interaction Raycast")]
        public float raycastDistance;
        public float raycastSphereRadius;
        public LayerMask interactableLayer;

        [SerializeField]private Camera cam;

        public BehaviorStateManager currentInteraction { get; private set; }

        public bool isInteracting { get; private set; }
        private float holdTimer = 0f;
        public float holdPercent { get; private set; }

        private InputAction dropInput;
        private ItemData activeHotbar;
        private BehaviorStateManager activeDisplayItem;
        private InventorySystem playerInventory;

        [SerializeField] private StaticInt activeIndex;

        public static Transform camTr;
        [SerializeField] Transform inventoryTr;

        private void Awake()
        {
            if (Controller == null) Controller = this;

            inputManger = new PlayerInput();

            camTr = cam.transform;
        }

        private void Start()
        {
            playerInventory = InventorySystem.PlayerInventory;
            playerInventory.OnHotbarSelection += SetActiveInteractable;
            holdTimer = 0f;
            isInteracting = false;
        }

        private void Update()
        {
            FindInteractable();
        }

        private void SetActiveInteractable()
        {
            if (activeIndex.Value > playerInventory.inventory.Count) return;

            if (activeDisplayItem != null) Destroy(activeDisplayItem.gameObject);

            if (activeIndex.Value == 0) { activeHotbar = null; }
            else
            {
                activeHotbar = playerInventory.inventory[activeIndex.Value - 1].data;

                activeDisplayItem = Instantiate(activeHotbar.Prefab, inventoryTr).GetComponent<BehaviorStateManager>();
                activeDisplayItem.SetNonInteractive();
            }
        }

        private void DropInput(InputAction.CallbackContext context)
        {
            if (activeIndex.Value <= 0 || activeHotbar == null) return;

            if (activeDisplayItem != null) Destroy(activeDisplayItem.gameObject);
            playerInventory.Remove(activeHotbar);
            activeIndex.Select(0);

            Vector3 _origin = inventoryTr.position + camTr.forward * 0.8f;

            BehaviorStateManager _interactable = Instantiate(activeHotbar.Prefab, _origin, Quaternion.identity).GetComponent<BehaviorStateManager>();
            
            _interactable.OnInteract(InteractionType.Drop);

            activeHotbar = null;
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
                BehaviorStateManager _currentInteraction = _hit.transform.gameObject.GetComponent<BehaviorStateManager>();
                if (_currentInteraction != null && currentInteraction != _currentInteraction)
                {
                    currentInteraction = _currentInteraction;
                }
            }
            else ResetInteraction();

            Debug.DrawRay(_ray.origin, _ray.direction * raycastDistance, _foundInteractable ? Color.green : Color.red);

            HandleInteraction();
        }

        public void ResetInteraction()
        {
            if (currentInteraction == null) return;

            currentInteraction = null;
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
            playerInventory.OnHotbarSelection -= SetActiveInteractable;
        }
    }
}