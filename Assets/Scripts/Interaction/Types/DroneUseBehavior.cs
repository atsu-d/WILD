using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "New Use Behavior", menuName = "ItemBehavior/Drone")]
    public class DroneUseBehavior : UseBehavior
    {
        public DroneController drone;

        public override void OnInteract()
        {
            drone.EnterDrone();
        }

        public override void OnUseEnd()
        {
        }
    }
}
