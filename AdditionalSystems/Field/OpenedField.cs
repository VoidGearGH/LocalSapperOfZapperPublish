using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
public class OpenedField : ISaveLoadObject
{
    public List<bool> Cells { get; private set; } = new();
    public OpenedField(string mode)
    {
        ComponentSaveId = mode switch
        {
            "Easy" => "EasyOpenedField",
            "Medium" => "MediumOpenedField",
            "Hard" => "HardOpenedField",
            _ => null
        };
    }
    public void Initialize(int totalCells)
    {
        Cells.Clear();
        for (int i = 0; i < totalCells; i++)
            Cells.Add(false);
    }
    public void UpdateFromProcessor(bool[,] opened, int n, int m)
    {
        Cells.Clear();
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                Cells.Add(opened[i, j]);
    }
    public override SaveLoadData GetSaveLoadData()
    {
        if (string.IsNullOrEmpty(ComponentSaveId))
            return null;

        return new OpenedFieldSLD(ComponentSaveId, Cells);
    }
    public override void RestoreValues(SaveLoadData loadData)
    {
        if (loadData?.Data == null || loadData.Data.Length == 0)
        {
            Debug.LogError("No data to restore OpenCells!");
            return;
        }

        try
        {
            var list = ((JArray)loadData.Data[0]).ToObject<List<bool>>();
            Cells.Clear();
            Cells.AddRange(list);
            IsRestored = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to restore OpenCells: {e.Message}");
        }
    }
}