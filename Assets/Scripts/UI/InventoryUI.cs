using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class InventoryUI : MonoBehaviour, IEventListener
    {
        public ManagerReference managers;
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
            foreach (InventoryItem _item in managers.InventoryManager.inventory)
            {
                int _position = managers.InventoryManager.inventory.IndexOf(_item) + 1;
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

        public void OnEventCalled()
        {
            managers.InventoryManager.OnInventoryUpdate += OnUpdateInventory;
        }

        private void OnEnable()
        {
            managers.managerEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            managers.managerEvent.UnregisterListener(this);
            managers.InventoryManager.OnInventoryUpdate -= OnUpdateInventory;
        }

    }

}

