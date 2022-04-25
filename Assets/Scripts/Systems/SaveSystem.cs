using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    public static string testDir = Application.persistentDataPath + "/saved/";

    /// <summary>
    /// Save JSON File from class data type
    /// </summary>
    public static void SaveToJson<T>(string fileName, T savedData)
    {
        if (!Directory.Exists(testDir))
        {
            Directory.CreateDirectory(testDir);
        }

        string json = JsonUtility.ToJson(savedData);

        File.WriteAllText($"{testDir}/{fileName}.json", json);
    }

    /// <summary>
    /// Save JSON File from List data type
    /// </summary>
    public static void SaveToJson<T>(string fileName, List<T> savedData)
    {
        if (!Directory.Exists(testDir))
        {
            Directory.CreateDirectory(testDir);
        }

        string json = JsonHelper.ListToJson(savedData.ToArray(), true);

        File.WriteAllText($"{testDir}/{fileName}.json", json);
    }

    /// <summary>
    /// Read JSON File to class data type
    /// </summary>
    public static void ReadJsonFile<T>(string fileName, out T data)
    {
        if (!File.Exists($"{testDir}/{fileName}.json"))
        {
            data = default(T);
            return;
        }

        string content = File.ReadAllText($"{testDir}/{fileName}.json");

        data = JsonUtility.FromJson<T>(content);
    }

    /// <summary>
    /// Read JSON File to List data type
    /// </summary>
    public static void ReadJsonFile<T>(string fileName, out List<T> data)
    {
        if (!File.Exists($"{testDir}/{fileName}.json"))
        {
            data = new List<T>();
            return;
        }

        string content = File.ReadAllText($"{testDir}/{fileName}.json");

        data = JsonHelper.JsonToList<T>(content).ToList();
    }
}

public static class JsonHelper
{
    public static T[] JsonToList<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        
        return wrapper.Items;
    }

    public static string ListToJson<T> (T[] array, bool prettyPrint = false)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;

        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    public class Wrapper<T>
    {
        public T[] Items;
    }
}
