using Unity.VisualScripting;
using UnityEngine;
public class Saver : MonoBehaviour
{
    [SerializeField] private FieldHolder _fieldHolder;
    private SaveLoadSystem _saveLoadSystem;

    private void Start()
    {
        if (_fieldHolder == null)
        {
            Debug.LogError("FieldHolder is not assigned in Saver inspector!", this);
            return;
        }

        _saveLoadSystem ??= new();
        _saveLoadSystem.AddToSaveLoad(_fieldHolder.Field);
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
}