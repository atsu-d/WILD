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
            ItemData _data = interactionManager.activeItem.Data;

            interactionManager.playerInventory.Remove(_data);
            interactionManager.worldContainer.Add(_data);
        }
    }
}