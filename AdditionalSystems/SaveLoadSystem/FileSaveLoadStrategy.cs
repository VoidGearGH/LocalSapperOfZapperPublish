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
            var newData = objectsToSave
                .Select(obj => obj.GetSaveLoadData())
                .Where(data => data != null)
                .ToList();

            List<SaveLoadData> existingData = new List<SaveLoadData>();
            if (File.Exists(SaveFilePath))
            {
                var json = File.ReadAllText(SaveFilePath);
                if (!string.IsNullOrEmpty(json))
                {
                    var existingFile = JsonConvert.DeserializeObject<SaveFile>(json);
                    if (existingFile.Data != null)
                        existingData = existingFile.Data.ToList();
                }
            }

            var dict = existingData.ToDictionary(d => d.Id);
            foreach (var data in newData)
            {
                dict[data.Id] = data;
            }

            var mergedData = dict.Values.ToList();
            var saveFile = new SaveFile(mergedData);
            var jsonOut = JsonConvert.SerializeObject(saveFile);
            File.WriteAllText(SaveFilePath, jsonOut);

            Debug.Log($"Saved {mergedData.Count} components (merged).");
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
    public void DeleteById(string id)
    {
        if (!File.Exists(SaveFilePath)) return;

        var existingFile = JsonConvert.DeserializeObject<SaveFile>(File.ReadAllText(SaveFilePath));
        if (existingFile.Data == null) return;

        var newData = existingFile.Data.Where(d => d.Id != id).ToList();
        File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(new SaveFile(newData)));
    }
    public void DeleteAll(IEnumerable<string> ids)
    {
        if (!File.Exists(SaveFilePath)) return;

        var existingFile = JsonConvert.DeserializeObject<SaveFile>(File.ReadAllText(SaveFilePath));
        if (existingFile.Data == null) return;

        var idsSet = new HashSet<string>(ids);
        var newData = existingFile.Data.Where(d => !idsSet.Contains(d.Id)).ToList();
        File.WriteAllText(SaveFilePath, JsonConvert.SerializeObject(new SaveFile(newData)));
    }
}