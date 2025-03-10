public class InventorySave
{
    private Inventory _inventory;

    public void SaveInventory()
    {
        _inventory = InventoryController.Instance.GetInventory();
        SaveSystem<Inventory>.Save(_inventory, "Inventory");
    }

    public void LoadInventory()
    {
        _inventory = SaveSystem<Inventory>.Load("Inventory");
        if (_inventory != null)
        {
            InventoryController.Instance.SetInventory(_inventory);
            UnityEngine.Debug.Log("Inventory loaded");
        }
        else
            UnityEngine.Debug.LogError("No Inventory found");
    }
}