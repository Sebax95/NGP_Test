using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Inventory/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    private Dictionary<int, ItemSO> itemDictionary;
    public List<ItemSO> allItems;

    private void OnEnable() => InitializeDictionary();

    private void InitializeDictionary()
    {
        itemDictionary = new Dictionary<int, ItemSO>();
        foreach (var item in allItems.Where(item => !itemDictionary.TryAdd(item.id, item)))
            Debug.LogWarning($"ID duplicated: {item.id}!");
    }

    public ItemSO GetItemSOById(int id) => itemDictionary.TryGetValue(id, out ItemSO itemSO) ? itemSO : null;
}