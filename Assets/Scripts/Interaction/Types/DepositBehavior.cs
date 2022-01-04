using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "New Deposit Behavior", menuName = "ItemBehavior/Deposit")]
    public class DepositBehavior : ItemBehavior
    {
        public void OnInteract()
        {
            ItemData _data = interactionData.activeItem;

            interactionData.playerInventory.Remove(_data);
            interactionData.worldContainer.Add(_data);

            interactionData.ClearHeldItem();
        }
    }
}