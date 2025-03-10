using System;
using UnityEngine;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public string description;
    public int quantity;
    public ItemType type;

    public Item(int id, string name, string description, int quantity, ItemType type, Sprite sprite)
    {
        this.id = id;
        this.name = name;
        this.description = description;
        this.quantity = quantity;
        this.type = type;
    }

    public Item(Item item)
    {
        id = item.id;
        name = item.name;
        description = item.description;
        quantity = item.quantity;
        type = item.type;
    }

    public Item()
    {
        type = ItemType.Empty;
        name = "";
        description = "";
        quantity = 0;
        id = -1;
    }
}
public enum ItemType
{
    Empty,
    Weapon,
    Consumable,
    Quest,
    Misc,
}