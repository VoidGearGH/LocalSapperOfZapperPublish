using UnityEngine;
public abstract class ISaveLoadObject
{
    public string ComponentSaveId;
    public abstract SaveLoadData GetSaveLoadData();
    public abstract void RestoreValues(SaveLoadData loadData);
}