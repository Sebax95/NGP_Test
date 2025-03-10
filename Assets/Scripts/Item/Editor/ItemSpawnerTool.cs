using UnityEditor;
using UnityEngine;

public class ItemSpawnerTool : EditorWindow
{
    private ItemSO selectedItemSO;
    private Vector3 spawnPosition;
    private Vector3 spawnScale = Vector3.one;

    [MenuItem("Tools/Item Tool/Item Spawner Tool")]
    public static void OpenItemSpawnerTool()
    {
        GetWindow<ItemSpawnerTool>("Item Spawner Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Item Spawner Tool", EditorStyles.boldLabel);

        selectedItemSO =
            (ItemSO)EditorGUILayout.ObjectField("Item ScriptableObject", selectedItemSO, typeof(ItemSO), false);

        if (selectedItemSO != null)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Prefab Preview:", EditorStyles.boldLabel);

            if (selectedItemSO.prefab != null)
                GUILayout.Label(AssetPreview.GetAssetPreview(selectedItemSO.prefab), GUILayout.Width(100),
                    GUILayout.Height(100));
            else
                EditorGUILayout.HelpBox("The selected ScriptableObject does not have an assigned prefab.",
                    MessageType.Warning);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Spawn Settings", EditorStyles.boldLabel);

            spawnPosition = EditorGUILayout.Vector3Field("Spawn Position", spawnPosition);

            spawnScale = EditorGUILayout.Vector3Field("Spawn Scale", spawnScale);

            GUILayout.Space(10);
            
            if (GUILayout.Button("Spawn Item in World"))
                SpawnItem();
        }
        else
        {
            EditorGUILayout.HelpBox("Select an ItemSO ScriptableObject to continue.", MessageType.Info);
        }
    }

    private void SpawnItem()
    {
        if (selectedItemSO == null || selectedItemSO.prefab == null)
        {
            Debug.LogError(
                "Cannot spawn the item because the selected ScriptableObject is invalid or does not have an assigned prefab.");
            return;
        }
        
        GameObject spawnedItem = (GameObject)PrefabUtility.InstantiatePrefab(selectedItemSO.prefab);
        spawnedItem.transform.position = spawnPosition;
        spawnedItem.transform.rotation = Quaternion.identity;
        spawnedItem.transform.localScale = spawnScale;

        PickableObject pickUp = spawnedItem.AddComponent<PickableObject>();
        pickUp.id = selectedItemSO.id;
        pickUp.type = selectedItemSO.type;
        pickUp.quantity = selectedItemSO.quantity;
        pickUp.description = selectedItemSO.description;
        pickUp.sprite = selectedItemSO.sprite;
        pickUp.name = selectedItemSO.name;

        Selection.activeGameObject = spawnedItem;

        Debug.Log(
            $"Item \"{selectedItemSO.name}\" spawned at position {spawnPosition}");
    }
}