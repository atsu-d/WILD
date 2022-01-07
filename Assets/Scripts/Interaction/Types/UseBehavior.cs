using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public abstract class UseBehavior : ItemBehavior
    {
        public GadgetType gadgetType;
        public abstract void OnUse();
        public abstract void OnUseEnd();
    }

    public enum GadgetType
    {
        Camera, Drone, Misc
    }
}
