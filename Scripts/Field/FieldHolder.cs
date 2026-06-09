using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
public class FieldHolder : MonoBehaviour
{
    [field: SerializeField] public Field Field { get; private set; }

    [SerializeField] private int n;
    [SerializeField] private int m;
    [SerializeField] private int bombsNum;

    private List<bool> loadedCells = null;
    private void Awake()
    {
        FieldGenerator fd = new FieldGenerator(n, m, bombsNum);
        var cells = fd.GetField().ToArray();

        Field = new Field(cells);

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
                if (ni >= 0 && ni < n && nj >= 0 && nj < m)
                {
                    int idx = ni * m + nj;
                    if (!excluded.Contains(idx))
                        excluded.Add(idx);
                }
            }
        }

        var generator = new FieldGenerator(n, m, bombsNum, excluded);
        var cells = generator.GetField().ToArray();
        Field = new Field(cells);
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
    public int GetBombsNum() { return bombsNum; }
}