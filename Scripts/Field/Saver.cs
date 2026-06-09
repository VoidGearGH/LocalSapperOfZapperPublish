using UnityEngine;

public class Saver : MonoBehaviour
{
    [SerializeField] private FieldHolder _fieldHolder;
    [SerializeField] private string sceneId; // "Easy" / "Medium" / "Hard"

    private SaveLoadSystem _saveLoadSystem;
    private bool _isDead = false;
    public bool IsLoadedGame { get; private set; } = false;
    private void Start()
    {
        if (_fieldHolder == null || _fieldHolder.Field == null)
        {
            Debug.LogError("Invalid FieldHolder or Field", this);
            return;
        }

        _saveLoadSystem = new SaveLoadSystem(sceneId);
        _saveLoadSystem.AddToSaveLoad(_fieldHolder.Field);

        if (_saveLoadSystem.HasSave(SaveType.File))
        {
            Load();
            IsLoadedGame = true;
        }

        FillingProcessor.OnDeath += DeleteSave;
    }
    public void DeleteSave()
    {
        _isDead = true;
        _saveLoadSystem.DeleteSave(SaveType.File);
    }

    private void OnDestroy()
    {
        FillingProcessor.OnDeath -= DeleteSave;
        if (!_isDead) Save();
    }
    public void Save()
    {
        _saveLoadSystem?.SaveGame(SaveType.File);
    }

    public void Load()
    {
        _saveLoadSystem.LoadGame(SaveType.File);
        _fieldHolder.UpdateCells();
    }

    public bool HasSave() => _saveLoadSystem != null && _saveLoadSystem.HasSave(SaveType.File);
}
