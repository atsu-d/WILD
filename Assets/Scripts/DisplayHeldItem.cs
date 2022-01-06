using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemSystem;
public class DisplayHeldItem : MonoBehaviour, IEventListener
{
    public ManagerReference managers;
    public InteractionData interactionData;

    [SerializeField] private IntVariable activeHotbar;

    private void SetActiveInteractable()
    {
        if (activeHotbar.value > managers.InventoryManager.inventory.Count) return;

        if (interactionData.heldItem != null) DestroyDisplay();

        if (activeHotbar.value == 0) { interactionData.ClearHeldItem(); }
        else
        {
            interactionData.SetHeldItem(Instantiate(interactionData.activeItem.Prefab, transform).GetComponent<ItemManager>());
            interactionData.heldItem.SetNonInteractive();
        }
    }

    private void DestroyDisplay()
    {
        Destroy(interactionData.heldItem.gameObject);
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
