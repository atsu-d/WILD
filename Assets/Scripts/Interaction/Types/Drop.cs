using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class Drop : ItemBehavior
    {
        private Rigidbody rb;
        private float throwForce = 3f;
        private Transform throwDir;
        public Vector3Variable playerVel;

        public bool removeVertical = false;
        public override void Initialize(ItemData _data)
        {
            base.Initialize(_data);

            rb = GetComponent<Rigidbody>();
            throwDir = InteractController.camTr;
        }

        public override void OnInteract()
        {
            rb.isKinematic = false;

            Vector3 _force = playerVel.value + (throwDir.forward * throwForce);
            if (removeVertical) _force.y = 0;

            rb.AddForce(_force, ForceMode.VelocityChange);
        }
    }
}
