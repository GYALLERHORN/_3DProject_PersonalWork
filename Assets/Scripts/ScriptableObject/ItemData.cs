using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Key,
    Destination
}

public enum ConsumableType
{
    health
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string name;
    public string description;
    public ItemType type;
    public Sprite icon;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;
}
