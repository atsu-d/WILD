using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "New Drop Behavior", menuName = "ItemBehavior/Drop")]
    public class DropBehavior : ItemBehavior
    {
        private float throwForce = 3f;

        public Vector3Variable playerVel;
        public bool removeVertical = false;

        public void OnInteract(Rigidbody _rb, Transform _throwDir, ItemData _data)
        {
            managers.InventoryManager.Remove(_data);
            interactionData.ClearActiveItem();

            _rb.isKinematic = false;

            Vector3 _force = playerVel.value + (_throwDir.forward * throwForce);
            if (removeVertical) _force.y = 0;

            _rb.AddForce(_force, ForceMode.VelocityChange);
        }
    }
}