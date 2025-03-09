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
        items = new List<Item>();
        maxSize = _max;
        this.currentSize = currentSize;
    }
    
    public void AddItem(Item item)
    {
        if (item == null) 
            return; 
        if (currentSize >= maxSize)
            return;
        Item existingItem = items.Find(i => i.id == item.id);
        if (existingItem != null)
            existingItem.quantity += item.quantity;
        else
        {
            items.Add(item);
            currentSize++;
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