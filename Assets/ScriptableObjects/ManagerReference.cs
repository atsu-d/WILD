using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[CreateAssetMenu(fileName = "New Reference Structure", menuName = "Manager/Reference")]
public class ManagerReference : ScriptableObject
{
    public InventorySystem InventoryManager { get; private set; }
    public InteractController InteractManager { get; private set; }

    public GameEvent managerEvent;

    #region Setters
    public void SetInventoryManager(InventorySystem _playerInventory) { InventoryManager = _playerInventory; }

    public void SetInteractManager(InteractController _interactManager) { InteractManager = _interactManager; }
    #endregion
}
