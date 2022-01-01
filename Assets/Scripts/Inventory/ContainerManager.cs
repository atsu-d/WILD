using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ItemType
{
    Food,
    Quest,
    All
}
namespace ItemSystem
{
    public class ContainerManager: MonoBehaviour
    {
        protected Dictionary<ItemData, InventoryItem> itemDictionary;
        [SerializeField] public List<InventoryItem> inventory { get; private set; }

        [SerializeField] private ItemType allowType;
        [SerializeField] private int capacity;

        protected virtual void Awake()
        {
            inventory = new List<InventoryItem>();
            itemDictionary = new Dictionary<ItemData, InventoryItem>();
        }

        public InventoryItem Get(ItemData _data)
        {
            if (itemDictionary.TryGetValue(_data, out InventoryItem value)) return value;
            return null;
        }

        public bool CanAdd(ItemData _data)
        {
            if (inventory.Count < capacity)
            {
                if (allowType == ItemType.All) return true;
                else return _data.Type == allowType;
            }

            return false;
        }

        public virtual void Add(ItemData _data)
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
        }

        public virtual void Remove(ItemData _data)
        {
            if (itemDictionary.TryGetValue(_data, out InventoryItem value))
            {
                value.RemoveFromStack();

                if (value.stackSize == 0)
                {
                    inventory.Remove(value);
                    itemDictionary.Remove(_data);
                }
            }
        }
    }
    
}
