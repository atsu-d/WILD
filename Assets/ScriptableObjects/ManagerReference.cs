using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

[CreateAssetMenu(fileName = "New Reference Structure", menuName = "Manager/Reference")]
public class ManagerReference : ScriptableObject
{
    public InventoryManager InventoryManager { get; private set; }
    public InteractController InteractManager { get; private set; }
    public InputManager InputManager;

    public GameEvent managerEvent;

    #region Setters
    public void SetInventoryManager(InventoryManager _playerInventory) { InventoryManager = _playerInventory; }

    public void SetInteractManager(InteractController _interactManager) { InteractManager = _interactManager; }
    #endregion
}
