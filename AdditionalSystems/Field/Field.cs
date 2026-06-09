using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Field : ISaveLoadObject
{
    public string ComponentSaveId { get; private set; }
    public List<bool> Cells { get; private set; } = new();

    public Field(bool[] cells, string sceneId)
    {
        ComponentSaveId = $"Field_{sceneId}";
        Cells.AddRange(cells);
    }

    public SaveLoadData GetSaveLoadData()
    {
        return new FieldSLD(ComponentSaveId, Cells);
    }

    public void RestoreValues(SaveLoadData loadData)
    {
        Cells.Clear();

        if (loadData?.Data == null)
        {
            Debug.LogError("Can't restore values!!!");
            return;
        }

        var cells = ((JArray)loadData.Data[0]).ToObject<List<bool>>();
        Cells.AddRange(cells);
    }
}
