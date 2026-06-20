using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class FieldHolder : MonoBehaviour
{
    [field: SerializeField] public Field Field { get; private set; }

    [SerializeField] private int _n;
    [SerializeField] private int _m;
    [SerializeField] private int _bombsNum;
    [SerializeField] private string _mode;

    private List<bool> loadedCells = null;
    private void Awake()
    {
        FieldGenerator fd = new FieldGenerator(_n, _m, _bombsNum);
        var cells = fd.GetField().ToArray();

        Field = new Field(cells, _mode);

        loadedCells = Field.Cells;
    }
    public void UpdateCells()
    {
        loadedCells = Field.Cells;
    }
    public void RegenerateFieldForSafeClick(int clickI, int clickJ)
    {
        List<int> excluded = new List<int>();
        for (int di = -1; di <= 1; di++)
        {
            for (int dj = -1; dj <= 1; dj++)
            {
                int ni = clickI + di;
                int nj = clickJ + dj;
                if (ni >= 0 && ni < _n && nj >= 0 && nj < _m)
                {
                    int idx = ni * _m + nj;
                    if (!excluded.Contains(idx))
                        excluded.Add(idx);
                }
            }
        }

        var generator = new FieldGenerator(_n, _m, _bombsNum, excluded);
        var cells = generator.GetField().ToArray();
        Field = new Field(cells, _mode);
        loadedCells = Field.Cells;
    }
    public List<bool> GetField()
    {
        if (loadedCells == null) return null;

        List<bool> toGetList = new List<bool>();
        foreach (bool cell in loadedCells)
        {
            toGetList.Add(cell);
        }

        return toGetList;
    }
    public int GetBombsNum() => _bombsNum;
    public string GetMode()
    {
        string toReturn = "";

        foreach(char c in _mode)
        {
            toReturn += c;
        }

        return toReturn;
    }
}