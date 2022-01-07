using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class ItemManager : MonoBehaviour
    {
        [SerializeField] private ItemData itemData;
        public ItemData Data => itemData;

        public UseBehavior UseAction; // E Key
        public DropBehavior DropAction; //R Key
        public DepositBehavior DepositAction;

        public PickUpBehavior PickUpAction; //Hold E

        protected int nonInteractableLayer = 30;
        private Collider col;
        public Rigidbody rb { get; private set; }

        [SerializeField] private WorldIconUI iconUI;

        private void Awake()
        {
            col = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            if (iconUI == null) GetComponentInChildren<WorldIconUI>();
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