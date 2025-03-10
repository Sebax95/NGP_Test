using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemSO : ScriptableObject
{
    public int id;
    public GameObject prefab;
    public string name;
    public string description;
    public int quantity;
    public ItemType type;
    public Sprite sprite;
}