using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;
public class FileSaveLoadStrategy : ISaveLoadStrategy
{
    private const string SaveFolderName = "Saves";

    private const string SaveFileName = "GameSaveFile.json";
    private static string SaveDataFolder => Path.Combine(Application.persistentDataPath, SaveFolderName);
    private static string SaveFilePath => Path.Combine(SaveDataFolder, SaveFileName);
    public void Save(IEnumerable<ISaveLoadObject> objectsToSave)
    {
        try
        {
            var serializedData = objectsToSave.Select(@object => @object.GetSaveLoadData()).ToList();

            if (!Directory.Exists(SaveDataFolder)) Directory.CreateDirectory(SaveDataFolder);

            var saveFile = new SaveFile(serializedData);
            var serializedSaveFile = JsonConvert.SerializeObject(saveFile);

            File.WriteAllText(SaveFilePath, serializedSaveFile);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }
    public SaveLoadData[] Load()
    {
        if (!File.Exists(SaveFilePath))
        {
            Debug.LogError($"File hasn't loaded, because {SaveFilePath} doesn't exist!!!");
            return null;
        }

        try
        {
            var serializedFile = File.ReadAllText(SaveFilePath);
            if (string.IsNullOrEmpty(serializedFile))
            {
                Debug.LogError($"Loaded file {SaveFilePath} is empty!!!");
                return null;
            }

            Debug.Log($"Save file to {SaveFilePath}");
            return JsonConvert.DeserializeObject<SaveFile>(serializedFile).Data.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}