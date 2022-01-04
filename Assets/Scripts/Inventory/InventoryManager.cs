using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ItemSystem
{
    public class InventoryManager : ContainerManager
    {
        public ManagerReference managers;
        public InteractionData interactionData;
        public PlayerInput playerInput;

        [SerializeField] private IntVariable activeHotbar;

        public event Action OnInventoryUpdate, OnHotbarSelection;
        private InputAction scrollInput, keyOne, keyTwo, keyThree, keyFour, keyFive, keySix, keySeven, keyEight, keyNine;

        protected override void Awake()
        {
            managers.SetInventoryManager(this);

            base.Awake();

            playerInput = new PlayerInput();
        }

        private void Start()
        {
            managers.managerEvent.CallEvent();
        }

        private void ScrollSelect(InputAction.CallbackContext context)
        {
            float _scrollDelta = -context.ReadValue<Vector2>().y;

            if (_scrollDelta == 0 || inventory.Count <= 0) return;

            if (_scrollDelta > 0) activeHotbar.Set(HotbarInt(activeHotbar.value, true));
            else activeHotbar.Set(HotbarInt(activeHotbar.value, false));

            if (OnHotbarSelection != null) OnHotbarSelection();
        }

        private void Select(InputAction.CallbackContext context)
        {
            int _keyPressed;
            int.TryParse(context.control.name, out _keyPressed);

            if (inventory.Count <= 0 || inventory.Count < _keyPressed) return;

            if (_keyPressed == activeHotbar.value)
            {
                activeHotbar.Set(0);
                interactionData.ClearActiveItem();
            }
            else
            {
                activeHotbar.Set(_keyPressed);
                SetItem(_keyPressed);
            }

            if (OnHotbarSelection != null) OnHotbarSelection();
        }

        private int HotbarInt(int _currentSelection, bool _positive)
        {
            int _nextIndex;

            if (_positive)
            {
                if (_currentSelection <= 0) 
                {
                    _nextIndex = 1;
                    SetItem(_nextIndex);
                    return _nextIndex; 
                }

                if (_currentSelection + 1 > inventory.Count)
                {
                    interactionData.ClearActiveItem();
                    return 0;
                }
                else 
                {
                    _nextIndex = _currentSelection + 1;
                    SetItem(_nextIndex);
                    return _nextIndex; 
                }
            }
            else
            {
                if (_currentSelection <= 0)
                {
                    _nextIndex = inventory.Count;
                    SetItem(_nextIndex);
                    return _nextIndex;
                }

                if (_currentSelection - 1 < 1)
                {
                    interactionData.ClearActiveItem();
                    return 0;
                }
                else
                {
                    _nextIndex = _currentSelection - 1;
                    SetItem(_nextIndex);
                    return _nextIndex;
                }
            }
        }

        private void SetItem(int _index) { interactionData.SetActiveItem(inventory[_index - 1].data); }

        public override void Add(ItemData _data)
        {
            base.Add(_data);
            if (OnInventoryUpdate != null) OnInventoryUpdate();
        }

        public override void Remove(ItemData _data)
        {
            base.Remove(_data);
            if (OnInventoryUpdate != null) OnInventoryUpdate();
        }

        private void OnEnable()
        {
            scrollInput = playerInput.Inventory.ScrollWheel;
            scrollInput.Enable();
            scrollInput.performed += ScrollSelect;

            #region Number Keys
            keyOne = playerInput.Inventory.Num1;
            keyOne.Enable();
            keyOne.performed += Select;
            keyTwo = playerInput.Inventory.Num2;
            keyTwo.Enable();
            keyTwo.performed += Select;
            keyThree = playerInput.Inventory.Num3;
            keyThree.Enable();
            keyThree.performed += Select;
            keyFour = playerInput.Inventory.Num4;
            keyFour.Enable();
            keyFour.performed += Select;
            keyFive = playerInput.Inventory.Num5;
            keyFive.Enable();
            keyFive.performed += Select;
            keySix = playerInput.Inventory.Num6;
            keySix.Enable();
            keySix.performed += Select;
            keySeven = playerInput.Inventory.Num7;
            keySeven.Enable();
            keySeven.performed += Select;
            keyEight = playerInput.Inventory.Num8;
            keyEight.Enable();
            keyEight.performed += Select;
            keyNine = playerInput.Inventory.Num9;
            keyNine.Enable();
            keyNine.performed += Select;
            #endregion
        }

        private void OnDisable()
        {
            scrollInput.Disable();

            #region Number Keys
            keyOne.Disable();
            keyTwo.Disable();
            keyThree.Disable();
            keyFour.Disable();
            keyFive.Disable();
            keySix.Disable();
            keySeven.Disable();
            keyEight.Disable();
            keyNine.Disable();
            #endregion
        }
    }
}
