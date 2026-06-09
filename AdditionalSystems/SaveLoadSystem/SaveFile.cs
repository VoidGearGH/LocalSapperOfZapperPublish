using System;
using System.Collections.Generic;

[System.Serializable]
public struct SaveFile
{
    public DateTime SaveTime { get; }
    public List<SaveLoadData> Data { get; private set; }
    public SaveFile(List<SaveLoadData> data) : this()
    {
        Data = data;
        SaveTime = DateTime.Now;
    }
}
