using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "New Interaction Data", menuName = "Manager/Interaction Data")]
    public class InteractionData : ScriptableObject
    {
        public InventoryManager playerInventory { get; private set; }
        public ItemData activeItem { get; private set; }
        public ItemManager heldItem { get; private set; }

        #region World Interaciton Data
        public ItemManager worldInteractable { get; private set; }
        public ContainerManager worldContainer { get; private set; }
        #endregion

        #region Setters 
        public void SetInventory(InventoryManager _manager) { playerInventory = _manager; }
        public void SetInteractable(ItemManager _interaction){ worldInteractable = _interaction; }
        public void SetContainer(ContainerManager _container) { worldContainer = _container; }
        public void SetActiveItem(ItemData _data) { activeItem = _data; }
        public void SetHeldItem(ItemManager _item) { heldItem = _item; }
        #endregion

        public void ClearContainer() { worldContainer = null; }
        public void ClearInteractable() { worldInteractable = null; }
        public void ClearActiveItem() { activeItem = null; }
        public void ClearHeldItem() { heldItem = null; }
    }

}