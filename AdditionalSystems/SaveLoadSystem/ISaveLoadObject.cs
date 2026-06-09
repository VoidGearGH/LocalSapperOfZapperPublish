using UnityEngine;
public interface ISaveLoadObject
{
    public string ComponentSaveId { get; }
    public SaveLoadData GetSaveLoadData();
    public void RestoreValues(SaveLoadData loadData);
}