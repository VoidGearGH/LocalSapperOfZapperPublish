using System.Collections.Generic;
public interface ISaveLoadStrategy
{
    public void Save(IEnumerable<ISaveLoadObject> objectsToSave);
    public SaveLoadData[] Load();
    public void DeleteById(string id);
    void DeleteAll(IEnumerable<string> ids);
}
