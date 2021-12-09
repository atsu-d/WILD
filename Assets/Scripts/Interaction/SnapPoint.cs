using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class SnapPoint : MonoBehaviour
    {
        private InventorySystem playerInventory;

        public void SetNextPoint(Transform _next)
        {
            transform.position = _next.position;
        }

        public void AddChild(Transform _item)
        {
            _item.SetParent(transform, false);
            _item.rotation = Quaternion.identity;
            _item.localPosition = Vector3.zero;
        }

        public void RemoveChild(GameObject _item)
        {
        }
    }
}

