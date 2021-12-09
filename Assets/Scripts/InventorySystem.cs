using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace ItemSystem
{
    public class InventorySystem : MonoBehaviour
    {
        public PlayerInput playerInput;
        public static InventorySystem PlayerInventory = null;
        private Dictionary<ItemData, InventoryItem> itemDictionary;
        [SerializeField]  public List<InventoryItem> inventory { get; private set; }

        public event Action OnInventoryUpdate;

        [SerializeField] private StaticInt activeHotbar;
        public event Action OnHotbarSelection;

        private InputAction scrollInput;
        private InputAction keyOne, keyTwo, keyThree, keyFour, keyFive, keySix, keySeven, keyEight, keyNine;

        private void Awake()
        {
            PlayerInventory = this;
            inventory = new List<InventoryItem>();
            itemDictionary = new Dictionary<ItemData, InventoryItem>();
            playerInput = new PlayerInput();
        }

        private void ScrollSelect(InputAction.CallbackContext context)
        {
            float _scrollDelta = -context.ReadValue<Vector2>().y;

            if (_scrollDelta == 0 || inventory.Count <= 0) return;

            if (_scrollDelta > 0) activeHotbar.Select(HotbarInt(activeHotbar.Value, true));
            else activeHotbar.Select(HotbarInt(activeHotbar.Value, false));

            if (OnHotbarSelection != null) OnHotbarSelection();
        }

        private void Select(InputAction.CallbackContext context)
        {
            int _keyPressed;
            int.TryParse(context.control.name, out _keyPressed);

            if (inventory.Count <= 0 || inventory.Count < _keyPressed) return;

            if (_keyPressed == activeHotbar.Value) activeHotbar.Select(0);
            else activeHotbar.Select(_keyPressed);

            if (OnHotbarSelection != null) OnHotbarSelection();
        }

        private int HotbarInt(int _currentSelection, bool _positive)
        {
            if (_positive)
            {
                if (_currentSelection <= 0) return 1;

                if (_currentSelection + 1 > inventory.Count) return 0;
                else return _currentSelection + 1;
            }
            else
            {
                if (_currentSelection <= 0) return inventory.Count;

                if (_currentSelection - 1 < 1) return 0;
                else return _currentSelection - 1;
            }
        }

        public InventoryItem Get(ItemData _data)
        {
            if (itemDictionary.TryGetValue(_data, out InventoryItem value)) return value;
            return null; 
        }

        public void Add(ItemData _data)
        {
            if (itemDictionary.TryGetValue(_data, out InventoryItem value))
            {
                value.AddToStack();
            }
            else
            {
                InventoryItem _newItem = new InventoryItem(_data);
                inventory.Add(_newItem);
                itemDictionary.Add(_data, _newItem);
            }

            if (OnInventoryUpdate != null) OnInventoryUpdate();
        }

        public void Remove(ItemData _data)
        {
            if (itemDictionary.TryGetValue(_data, out InventoryItem value))
            {
                value.RemoveFromStack();

                if(value.stackSize == 0)
                {
                    inventory.Remove(value);
                    itemDictionary.Remove(_data);
                }
            }

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
