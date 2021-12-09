using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem 
{
    public class PickUp : HoldBehavior
    {
        //struct requirements for interactions
        public override void OnInteract()
        {
            InventorySystem.PlayerInventory.Add(itemData);
            Destroy(this.gameObject);
        }
    }
}