using System.IO;
using Unity.VisualScripting;
using UnityEngine;
public class Saver : MonoBehaviour
{
    [SerializeField] private FieldHolder _fieldHolder;
    private SaveLoadSystem _saveLoadSystem;

    private const string SaveFolderName = "Saves";

    private const string SaveFileName = "GameSaveFile.json";
    private static string SaveDataFolder => Path.Combine(Application.persistentDataPath, SaveFolderName);
    private static string SaveFilePath => Path.Combine(SaveDataFolder, SaveFileName);
    private void Start()
    {
        if (_fieldHolder == null)
        {
            Debug.LogError("FieldHolder is not assigned in Saver inspector!", this);
            return;
        }

        _saveLoadSystem ??= new();
        _saveLoadSystem.AddToSaveLoad(_fieldHolder.Field);

        if(File.Exists(SaveFilePath))
            Load();
    }
    public void Save()
    {
        _saveLoadSystem.SaveGame(SaveType.File);
    }
    public void Load()
    {
        _saveLoadSystem.LoadGame(SaveType.File);

        _fieldHolder.UpdateCells();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
}