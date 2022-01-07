using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemSystem
{
    [CreateAssetMenu(fileName = "Interaction Manager", menuName = "Manager/Interaction Manager")]
    public class InteractionManager : ScriptableObject
    {
        public InventoryManager playerInventory { get; private set; }
        public ItemManager activeItem { get; private set; }

        #region World Interaciton Data
        public ItemManager worldInteractable { get; private set; }
        public ContainerManager worldContainer { get; private set; }
        #endregion

        #region Instantiate Variables
        [SerializeField] private float spawnForwardDistance = 0.8f;
        private Transform inventoryTr;
        #endregion

        public GameEvent OnDiscardItem;

        #region Methods
        public void OnInteract(InteractionType _type)
        {
            switch (_type)
            {
                case InteractionType.PickUp:
                    worldInteractable.PickUpAction.OnInteract(worldInteractable.Data, worldInteractable.gameObject);
                    break;

                case InteractionType.Drop:
                    ItemManager _dropItem = SpawnItem();
                    _dropItem.DropAction.OnInteract(_dropItem.rb, inventoryTr, _dropItem.Data);

                    if (OnDiscardItem != null)
                        OnDiscardItem.CallEvent();
                    break;

                case InteractionType.Deposit:
                    activeItem.DepositAction.OnInteract();

                    if (OnDiscardItem != null)
                        OnDiscardItem.CallEvent();
                    break;

                case InteractionType.Use:
                    UseItem(activeItem.UseAction.gadgetType);
                    break;
            }
        }

        private void UseItem(GadgetType _gadget)
        {
            switch (_gadget)
            {
                case GadgetType.Camera:
                    break;

                case GadgetType.Drone:
                    ItemManager _drone = SpawnItem();
                    _drone.UseAction.OnUse();

                    if (OnDiscardItem != null)
                        OnDiscardItem.CallEvent();
                    break;

            }
        }

        public ItemManager SpawnItem()
        {
            Vector3 _spawnLocation = inventoryTr.position + (inventoryTr.forward * spawnForwardDistance);
            return Instantiate(activeItem.Data.Prefab, _spawnLocation, Quaternion.identity).GetComponent<ItemManager>();
        }

        #endregion

        #region Setters 
        public void SetInventory(InventoryManager _manager) { playerInventory = _manager; }
        public void SetInteractable(ItemManager _interaction) { worldInteractable = _interaction; }
        public void SetContainer(ContainerManager _container) { worldContainer = _container; }
        public void SetActiveItem(ItemManager _data) { activeItem = _data; }
        public void SetInventoryTransform(Transform _tr) { inventoryTr = _tr; }
        #endregion

        #region Clearers
        public void ClearContainer() { worldContainer = null; }
        public void ClearInteractable() { worldInteractable = null; }
        public void ClearActiveItem() { activeItem = null; }
        #endregion
    }

    public enum InteractionType
    {
        PickUp, Drop, Deposit, Use
    }
}