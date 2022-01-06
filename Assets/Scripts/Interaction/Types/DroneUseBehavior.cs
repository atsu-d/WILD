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

        public override void OnInteract(ItemData _data, Rigidbody _rb)
        {
            Debug.Log("Called");
            interactionData.playerInventory.Remove(_data);
            interactionData.ClearActiveItem();

            _rb.isKinematic = false;

            drone.EnterDrone();
            inUse = true;
        }

        public override void OnUseEnd()
        {
            inUse = false;
        }
    }
}
