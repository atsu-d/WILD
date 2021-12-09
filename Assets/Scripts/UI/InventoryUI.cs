using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private GameObject slotPrefab;

        private void OnUpdateInventory()
        {
            foreach(Transform _t in transform)
            {
                Destroy(_t.gameObject);
            }

            DrawInventory();
        }

        public void DrawInventory()
        {
            foreach (InventoryItem _item in InventorySystem.PlayerInventory.inventory)
            {
                int _position = InventorySystem.PlayerInventory.inventory.IndexOf(_item) + 1;
                AddInventorySlot(_item, _position);
            }
        }

        public void AddInventorySlot(InventoryItem _item, int _inventoryPos)
        {
            GameObject _obj = Instantiate(slotPrefab);
            _obj.transform.SetParent(transform, false);

            SlotUI _slot = _obj.GetComponent<SlotUI>();
            _slot.Set(_item, _inventoryPos);
        }

        private void Start()
        {
            InventorySystem.PlayerInventory.OnInventoryUpdate += OnUpdateInventory;
        }

        private void OnDestroy()
        {
            InventorySystem.PlayerInventory.OnInventoryUpdate -= OnUpdateInventory;
        }
    }

}

