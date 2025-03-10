using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem<T> where T : class
{
    private static string GetPath(string fileName) => Path.Combine(Application.persistentDataPath, fileName + ".dat");

    public static void Save(T data, string fileName)
    {
        string path = GetPath(fileName);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log($"Save System: Saved in {path}");
    }

    public static T Load(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            T data = formatter.Deserialize(stream) as T;
            stream.Close();
            return data;
        }

        Debug.LogWarning($"file not found in {path}");
        return null;
    }
}