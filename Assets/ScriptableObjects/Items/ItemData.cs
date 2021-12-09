using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Data")]
    [SerializeField] private int id;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int maxStack;
    public int ID => id;
    public string ItemName => itemName;
    public Sprite Icon => icon;
    public GameObject Prefab => prefab;
    public int MaxStack => maxStack;

    [Space(5)]

    [Header("Economical Data")]
    [SerializeField] private float purchaseValue;
    [SerializeField] private float saleValue;
    public float PurchaseValue => purchaseValue;
    public float SaleValue => saleValue;

    public void SetSaleValue(float _newValue)
    {
        saleValue = _newValue;
    }

    public void SetPurchaseValue(float _newValue)
    {
        purchaseValue = _newValue;
    }
}
