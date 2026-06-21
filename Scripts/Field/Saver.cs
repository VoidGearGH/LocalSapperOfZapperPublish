using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
public class Saver : MonoBehaviour
{
    [SerializeField] private FieldHolder _fieldHolder;
    [SerializeField] private FillingProcessor _fillingProcessor;
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
        _saveLoadSystem.AddToSaveLoad(_fieldHolder.OpenedField);

        if (File.Exists(SaveFilePath))
        {
            Load();
        }
        
        Save();
    }
    public void Save()
    {
        var flat = _fillingProcessor.GetOpenedCellsFlat();
        int openedCount = flat.Count(b => b);
        Debug.Log($"Saving {openedCount} opened cells out of {flat.Count}");
        _fieldHolder.UpdateOpenCellsFromProcessor(flat);
        _saveLoadSystem.SaveGame(SaveType.File);
    }
    public void Load()
    {
        Debug.Log("Loading save...");
        _saveLoadSystem.LoadGame(SaveType.File);

        _fieldHolder.UpdateCells();
        _fillingProcessor.RefreshFieldFromHolder();
        _fieldHolder.ApplyOpenedCellsToProcessor(_fillingProcessor);

        var flat = _fillingProcessor.GetOpenedCellsFlat();
        int openedCount = flat.Count(b => b);
        Debug.Log($"Loaded {openedCount} opened cells.");
    }
    private void OnApplicationQuit()
    {
        Save();
    }

    private void OnEnable()
    {
        FillingProcessor.OnDeath += DeleteSave;
        FillingProcessor.OnWin += DeleteSave;
    }
    private void OnDisable()
    {
        FillingProcessor.OnDeath -= DeleteSave;
        FillingProcessor.OnWin -= DeleteSave;
    }
    public void DeleteSave()
    {
        _saveLoadSystem.DeleteSave();
    }
}