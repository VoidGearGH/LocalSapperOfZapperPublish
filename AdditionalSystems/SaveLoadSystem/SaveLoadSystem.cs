using UnityEngine;
using System.Collections.Generic;
using System;

public enum SaveType
{
    File, SteamCloud
}
public class SaveLoadSystem
{
    private readonly FileSaveLoadStrategy fileSaveLoadStrategy;

    private Dictionary<string, ISaveLoadObject> componentsIdToSaveObject = new();

    public SaveLoadSystem(string sceneId)
    {
        fileSaveLoadStrategy = new FileSaveLoadStrategy($"Save_{sceneId}.json");
    }

    public void DeleteSave(SaveType saveType)
    {
        var strategy = saveType switch
        {
            SaveType.File => fileSaveLoadStrategy,
            _ => throw new NotImplementedException()
        };
        strategy.DeleteSave();
    }
    public void AddToSaveLoad(ISaveLoadObject saveLoadObject)
        => componentsIdToSaveObject[saveLoadObject.ComponentSaveId] = saveLoadObject;

    public void SaveGame(SaveType saveType)
    {
        var strategy = saveType switch
        {
            SaveType.File => fileSaveLoadStrategy,
            _ => throw new NotImplementedException()
        };

        SaveAll(strategy);
    }

    public void LoadGame(SaveType saveType)
    {
        var strategy = saveType switch
        {
            SaveType.File => fileSaveLoadStrategy,
            _ => throw new NotImplementedException()
        };

        Load(strategy);
    }

    public bool HasSave(SaveType saveType)
    {
        var strategy = saveType switch
        {
            SaveType.File => fileSaveLoadStrategy,
            _ => throw new NotImplementedException()
        };

        return strategy.HasSave();
    }

    private void SaveAll(ISaveLoadStrategy strategy) => Save(strategy, componentsIdToSaveObject.Values);

    private void Save(ISaveLoadStrategy strategy, IEnumerable<ISaveLoadObject> data) => strategy.Save(data);

    private void Load(ISaveLoadStrategy strategy)
    {
        var loadedData = strategy.Load();

        if (loadedData == null) return;

        foreach (var data in loadedData)
        {
            var objectId = data.Id;
            if (!componentsIdToSaveObject.ContainsKey(objectId))
            {
                Debug.LogError($"Can't restore data for object with id {objectId}");
                continue;
            }

            componentsIdToSaveObject[objectId].RestoreValues(data);
        }
    }
}
