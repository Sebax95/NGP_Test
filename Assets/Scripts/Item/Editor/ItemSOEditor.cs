using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemSO))]
public class ItemSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ItemSO itemSO = (ItemSO)target;

        EditorGUILayout.LabelField("Item Details", EditorStyles.boldLabel);

        itemSO.id = EditorGUILayout.IntField("ID", itemSO.id);
        itemSO.name = EditorGUILayout.TextField("Name", itemSO.name);
        itemSO.description = EditorGUILayout.TextField("Description", itemSO.description);
        itemSO.quantity = EditorGUILayout.IntField("Quantity", itemSO.quantity);
        itemSO.type = (ItemType)EditorGUILayout.EnumPopup("Type", itemSO.type);

        itemSO.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", itemSO.prefab, typeof(GameObject), false);
        itemSO.sprite = (Sprite)EditorGUILayout.ObjectField("Sprite", itemSO.sprite, typeof(Sprite), false);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Previews", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (itemSO.sprite != null)
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Label("Sprite:");
            GUILayout.Label(AssetPreview.GetAssetPreview(itemSO.sprite), GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.EndVertical();
        }

        if (itemSO.prefab != null)
        {
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Label("Prefab:");
            GUILayout.Label(AssetPreview.GetAssetPreview(itemSO.prefab), GUILayout.Width(100), GUILayout.Height(100));
            GUILayout.EndVertical();
        }

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(itemSO);
        }
    }
}