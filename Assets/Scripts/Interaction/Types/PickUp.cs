using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem 
{
    public class PickUp : HoldBehavior
    {
        public ManagerReference managers;
        //struct requirements for interactions
        public override void OnInteract()
        {
            managers.InventoryManager.Add(itemData);
            Destroy(this.gameObject);
        }
    }
}