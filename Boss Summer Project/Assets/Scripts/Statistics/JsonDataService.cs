using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System;

public class JsonDataService : IDataService
{
    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Application.persistentDataPath + relativePath;

        try
        {
            if (File.Exists(path))
            {
                Debug.Log("It works; there is an existing data file. Deleting old file");
                File.Delete(path);
            }

            else
            {
                Debug.Log("Creating file for the first time");
            }

            // Using keyword 
            using FileStream stream = File.Create(path);
            stream.Close();
            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
            return true;
        }

        catch (Exception e)
        {
            Debug.LogError($"Error due to {e.Message} {e.StackTrace}");
            return false;
        }
    }

    public T LoadData<T>(string relativePath)
    {
        string path = Application.persistentDataPath + relativePath;

        if(!File.Exists(path))
        {
            Debug.LogError($"Cannot load file at {path}");
            throw new FileNotFoundException($"Cannot find file at {path}");
        }
        
        try 
        {
            T data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return data;
        }

        catch(Exception e)
        {
            Debug.LogError($"Error due to {e.Message} {e.StackTrace}");
            throw e;
        }
    }

}
