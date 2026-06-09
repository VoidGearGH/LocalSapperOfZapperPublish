[System.Serializable]
public class SaveLoadData
{
    public SaveLoadData(string id, object[] data)
    {
        Id = id;
        Data = data;
    }
    public string Id { get; private set; }
    public object[] Data { get; private set; }
}