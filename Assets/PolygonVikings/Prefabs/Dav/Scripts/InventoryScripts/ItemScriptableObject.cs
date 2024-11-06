using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { Default, Food, Weapon, Intrument}
public class ItemScriptableObject : ScriptableObject
{

    public string itemName;
    public GameObject itemPrefab;
    public Sprite icon;
    public ItemType itemType;
    public int maximumAmount;
    public string itemDescription;
    public bool isConsumeable;
    
    [Header("Consumable Characteristics")]
    public float changeHealth;
    public float changeHunger;
    public float changeThirst;


}
