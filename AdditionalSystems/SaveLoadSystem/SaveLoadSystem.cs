using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum SaveType
{
    File, SteamCloud
}
public class SaveLoadSystem
{
    private readonly FileSaveLoadStrategy fileSaveLoadStrategy = new();

    private Dictionary<string, ISaveLoadObject> componentsIdToSaveObject = new();

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
                Debug.LogWarning($"Can't restore data for object with id {objectId}");
                continue;
            }

            componentsIdToSaveObject[objectId].RestoreValues(data);
        }
    }
    public void DeleteSave()
    {
        fileSaveLoadStrategy.DeleteById(
            componentsIdToSaveObject.Keys.First()
        );
    }
}