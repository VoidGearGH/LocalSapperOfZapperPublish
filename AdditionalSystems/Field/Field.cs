using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
public class Field : ISaveLoadObject
{
    public List<bool> Cells { get; private set; } = new();
    public Field(bool[] cells, string mode)
    {
        Cells.AddRange(cells);

        ComponentSaveId = mode switch
        {
            "Easy" => "EasyField",
            "Medium" => "MediumField",
            "Hard" => "HardField",
            _ => null
        };
    }
    public override SaveLoadData GetSaveLoadData()
    {
        if(string.IsNullOrEmpty(ComponentSaveId))
            return null;

        return new FieldSLD(ComponentSaveId, Cells);
    }
    public override void RestoreValues(SaveLoadData loadData)
    {
        Debug.Log($"[RestoreValues] Data[0] type: {loadData.Data[0]?.GetType()}");
        Debug.Log($"[RestoreValues] Data[0] value: {loadData.Data[0]}");
        Cells.Clear();

        if (loadData?.Data == null)
        {
            Debug.LogError("Can't restore values!!!");
            return;
        }

        var cells = ((JArray)loadData.Data[0]).ToObject<List<bool>>();
        Cells.AddRange(cells);

        Debug.Log($"[Field.RestoreValues] Restored, first 5: {string.Join(",", Cells.GetRange(0, 5))}");
        
        IsRestored = true;
    }
}