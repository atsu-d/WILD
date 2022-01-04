using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem 
{
    [CreateAssetMenu(fileName = "New Pick Up Behavior", menuName = "ItemBehavior/Pick Up")]
    public class PickUpBehavior : HoldBehavior
    {
        public void OnInteract(ItemData _data, GameObject _itemObject)
        {
            interactionData.playerInventory.Add(_data);
            Destroy(_itemObject);
        }
    }
}