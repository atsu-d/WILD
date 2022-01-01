using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        public ItemData Data => itemData;

        [SerializeField] private ItemBehavior primaryTapBehavior; // E Key
        [SerializeField] private HoldBehavior holdBehavior;
        [SerializeField] private ItemBehavior secondaryTapBehavior; //R Key

        protected int nonInteractableLayer = 30;
        private Collider col;
        public bool HoldInteract => holdBehavior != null;
        public float HoldDuration => holdBehavior.holdDuration;

        [SerializeField] private WorldIconUI iconUI;

        private void Awake()
        {
            if (primaryTapBehavior != null) primaryTapBehavior.Initialize(itemData);
            if (holdBehavior != null) holdBehavior.Initialize(itemData);
            if (secondaryTapBehavior != null) secondaryTapBehavior.Initialize(itemData);
            col = GetComponent<Collider>();
        }

        private void Start()
        {
            if (iconUI == null) GetComponentInChildren<WorldIconUI>();
        }

        public void OnInteract(InteractionType _input)
        {
            switch (_input)
            {
                case InteractionType.Tap:
                    primaryTapBehavior.OnInteract();
                    break;
                case InteractionType.Hold:
                    holdBehavior.OnInteract();
                    break;
                case InteractionType.Drop:
                    secondaryTapBehavior.OnInteract();
                    break;
            }
        }

        private void ZeroTransforms()
        {
            transform.localPosition = Vector3.zero;
            transform.rotation = transform.parent.rotation;
        }

        public virtual void SetNonInteractive()
        {
            gameObject.layer = nonInteractableLayer;
            col.enabled = false;

            ZeroTransforms();
            TurnOffWorldIcon();
        }

        private void TurnOffWorldIcon()
        {
            iconUI.gameObject.SetActive(false);
        }

        public void ChangeInteractUI(bool _isHold)
        {
            iconUI.SwapImage(_isHold);
        }
    }
}

public enum InteractionType
{
    Tap, Hold, Drop
}
