using UnityEngine;
public abstract class ISaveLoadObject
{
    public string ComponentSaveId;
    public bool IsRestored { get; protected set; } = false;
    public abstract SaveLoadData GetSaveLoadData();
    public abstract void RestoreValues(SaveLoadData loadData);
}