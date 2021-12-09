using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class Drop : ItemBehavior
    {
        private Rigidbody rb;
        private float throwForce = 7f;
        private Transform throwDir;

        public override void Initialize(ItemData _data)
        {
            base.Initialize(_data);

            rb = GetComponent<Rigidbody>();
            throwDir = InteractController.camTr;
        }

        public override void OnInteract()
        {
            rb.isKinematic = false;

            rb.AddForce(throwDir.forward * throwForce, ForceMode.Impulse);
        }
    }
}
