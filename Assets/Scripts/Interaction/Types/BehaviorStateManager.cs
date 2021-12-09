using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class BehaviorStateManager : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;

        [SerializeField] private ItemBehavior primaryTapBehavior; // E Key
        [SerializeField] private HoldBehavior holdBehavior;
        [SerializeField] private ItemBehavior secondaryTapBehavior; //R Key

        [SerializeField] private Transform upperSnap;
        public Transform UpperSnap => upperSnap;
        protected int nonInteractableLayer = 30;

        public bool HoldInteract => holdBehavior != null;
        public float HoldDuration => holdBehavior.holdDuration;

        [SerializeField] private WorldIconUI iconUI;

        private void Awake()
        {
            if (primaryTapBehavior != null) primaryTapBehavior.Initialize(itemData);
            if (holdBehavior != null) holdBehavior.Initialize(itemData);
            if (secondaryTapBehavior != null) secondaryTapBehavior.Initialize(itemData);
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

        public virtual void SetNonInteractive()
        {
            gameObject.layer = nonInteractableLayer;
        }

        public virtual void TurnOffWorldIcon()
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
