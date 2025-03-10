using System;
using System.Collections.Generic;

[Serializable]
public class Inventory
{
    public List<Item> items;
    public int maxSize = 10;
    public int currentSize = 0;

    public Inventory(int _max, int currentSize)
    {
        maxSize = _max;
        this.currentSize = currentSize;

        items = new List<Item>(maxSize);
        for (int i = 0; i < maxSize; i++)
            items.Add(new Item()); 
    }

    public void AddItem(Item item)
    {
        if (item.type == ItemType.Empty)
            return;

        if (currentSize >= maxSize)
            return;

        Item existingItem = items.Find(i => i != null && i.id == item.id);
        if (existingItem != null)
            existingItem.quantity += item.quantity; 
        else
        {
            int emptyIndex = items.FindIndex(i => i == null || i.type == ItemType.Empty);
            if (emptyIndex != -1)
            {
                items[emptyIndex] = new Item(item);
                currentSize++;
            }
        }
    }
    
    public void RemoveItem(int itemId, int quantityToRemove)
    {
        if (quantityToRemove <= 0)
            return;
        
        Item existingItem = items.Find(i => i.id == itemId);
        if (existingItem != null)
        {
            existingItem.quantity -= quantityToRemove;
            
            if (existingItem.quantity <= 0)
            {
                existingItem.quantity = 0;
                
                items.Remove(existingItem);
                currentSize--;
            }
        }
    }

    public void SortByName() => items.Sort((x, y) => String.Compare(x.name, y.name, StringComparison.Ordinal));

    public void SortByQuantity() => items.Sort((x, y) => x.quantity.CompareTo(y.quantity));

    public void SortByType() => items.Sort((x, y) => x.type.CompareTo(y.type));
}