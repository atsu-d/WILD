using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public abstract class UseBehavior : ItemBehavior
    {
        public abstract void OnInteract();
        public abstract void OnUseEnd();
    }
}
