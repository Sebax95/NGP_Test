using UnityEditor;
using UnityEngine;

public class ItemToolEditor : EditorWindow
{
    private string itemName;
    private string itemDescription;
    private int itemID;
    private ItemType itemType;
    private Sprite selectedSprite;
    private GameObject selectedPrefab;

    [MenuItem("Tools/Item Tool/Create New Item")]
    public static void OpenItemTool()
    {
        GetWindow<ItemToolEditor>("Item Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Create New Item ScriptableObject", EditorStyles.boldLabel);

        itemName = EditorGUILayout.TextField("Item Name", itemName);
        itemDescription = EditorGUILayout.TextField("Description", itemDescription);
        itemID = EditorGUILayout.IntField("Item ID", itemID);
        itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", itemType);
        selectedSprite = (Sprite)EditorGUILayout.ObjectField("Sprite", selectedSprite, typeof(Sprite), false);
        selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", selectedPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Create Item"))
            CreateNewItem();
    }

    private void CreateNewItem()
    {
        ItemSO newItem = CreateInstance<ItemSO>();
        newItem.name = itemName;
        newItem.description = itemDescription;
        newItem.id = itemID;
        newItem.type = itemType;
        newItem.sprite = selectedSprite;
        newItem.prefab = selectedPrefab;
        newItem.quantity = 1;

        string resourcesPath = "Assets/Resources";
        if (!AssetDatabase.IsValidFolder(resourcesPath))
            AssetDatabase.CreateFolder("Assets", "Resources/Item Data");

        string filePath = $"{resourcesPath}/Item Data/{itemName}.asset";
        AssetDatabase.CreateAsset(newItem, filePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Item ScriptableObject created at {filePath}.");
    }
    
}