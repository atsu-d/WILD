using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    public class HoldBehavior : ItemBehavior
    {
        [SerializeField] private float HoldDuration;
        public float holdDuration => HoldDuration;
    }
}