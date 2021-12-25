using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ItemSystem;

namespace ItemSystem
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI hotbarText;
        private float selectScale = 1.5f;

        public int hotbarNum { get; private set; }
        [SerializeField] private IntVariable activeHotbar;

        public void Set(InventoryItem _item, int _inventoryNumber)
        {
            hotbarNum = _inventoryNumber;
            hotbarText.text = _inventoryNumber.ToString();
            icon.sprite = _item.data.Icon;
            //weightText = 
        }

        private void SetScale()
        {
            if (hotbarNum == activeHotbar.value)
            {
                icon.rectTransform.localScale = Vector3.one * selectScale;
            }
            else if (icon.rectTransform.localScale != Vector3.one) icon.rectTransform.localScale = Vector3.one;
        }

        private void Start()
        {
            InventorySystem.PlayerInventory.OnHotbarSelection += SetScale;
        }

        private void OnDestroy()
        {
            InventorySystem.PlayerInventory.OnHotbarSelection -= SetScale;
        }
    }

}
