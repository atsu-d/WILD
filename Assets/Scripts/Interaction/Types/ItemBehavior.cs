using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public abstract class ItemBehavior : MonoBehaviour
    {
        public ItemData itemData { get; private set; }
       
        public virtual void Initialize(ItemData _data)
        {
            itemData = _data;
        }

        public abstract void OnInteract();
    }
}