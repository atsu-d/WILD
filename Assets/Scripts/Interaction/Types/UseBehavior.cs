using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public abstract class UseBehavior : ItemBehavior
    {
        public abstract void OnInteract(ItemData _data, Rigidbody _rb);
        public abstract void OnUseEnd();
    }
}
