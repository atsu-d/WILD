using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;

public class DisplayHeldItem : MonoBehaviour, IEventListener
{
    public ManagerReference managers;
    public InteractionManager interactionManager;

    [SerializeField] private IntVariable activeHotbar;

    private void SetActiveInteractable()
    {
        if (activeHotbar.value > managers.InventoryManager.inventory.Count) return;

        if (interactionManager.activeItem != null)
            DestroyDisplay();

        if (activeHotbar.value == 0)
            DestroyDisplay();
        else
        {
            interactionManager.SetActiveItem(Instantiate(interactionManager.playerInventory.inventory[activeHotbar.value - 1].data.Prefab, transform).GetComponent<ItemManager>());
            interactionManager.activeItem.SetNonInteractive();
        }
    }

    private void DestroyDisplay()
    {
        if (interactionManager.activeItem == null)
            return;

        Destroy(interactionManager.activeItem.gameObject);
        interactionManager.ClearActiveItem();
    }

    public void OnEventCalled(int _id)
    {
        switch (_id)
        {
            case 1:
                managers.InventoryManager.OnHotbarSelection += SetActiveInteractable;
                break;
            case 2:
                DestroyDisplay();
                break;
        }
    }

    private void OnEnable()
    {
        managers.managerEvent.RegisterListener(this);
        interactionManager.OnDiscardItem.RegisterListener(this);
    }

    private void OnDisable()
    {
        managers.managerEvent.UnregisterListener(this);
        interactionManager.OnDiscardItem.UnregisterListener(this);

        managers.InventoryManager.OnHotbarSelection -= SetActiveInteractable;
    }

}
