using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
public class DisplayHeldItem : MonoBehaviour, IEventListener
{
    public ManagerReference managers;
    private BehaviorStateManager activeItem;
    [SerializeField] private IntVariable activeHotbar;

    private void SetActiveInteractable()
    {
        if (activeHotbar.value > managers.InventoryManager.inventory.Count) return;

        if (activeItem != null) DestroyDisplay();

        if (activeHotbar.value == 0) { activeItem = null; }
        else
        {
            ItemData _newItem = managers.InventoryManager.inventory[activeHotbar.value - 1].data;
            activeItem = Instantiate(_newItem.Prefab, transform).GetComponent<BehaviorStateManager>();
            activeItem.SetNonInteractive();
        }
    }

    private void DestroyDisplay()
    {
        Destroy(activeItem.gameObject);
        activeItem = null;
    }

    public void OnEventCalled()
    {
        managers.InventoryManager.OnHotbarSelection += SetActiveInteractable;
        managers.InteractManager.OnDiscardItem += DestroyDisplay;
    }

    private void OnEnable()
    {
        managers.managerEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        managers.managerEvent.UnregisterListener(this);
        managers.InventoryManager.OnHotbarSelection -= SetActiveInteractable;
        managers.InteractManager.OnDiscardItem -= DestroyDisplay;
    }

}
