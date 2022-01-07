using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "New Use Behavior", menuName = "ItemBehavior/Drone")]
    public class DroneUseBehavior : UseBehavior
    {
        public bool inUse { get; private set; }
        public DroneController drone;

        public void Awake()
        {
            gadgetType = GadgetType.Drone;
        }

        public override void OnUse()
        {
            ItemManager _drone = interactionManager.activeItem;

            interactionManager.playerInventory.Remove(_drone.Data);
            drone.rb.isKinematic = false;

            drone.EnterDrone();
            inUse = true;
        }

        public override void OnUseEnd()
        {
            inUse = false;
        }
    }
}
