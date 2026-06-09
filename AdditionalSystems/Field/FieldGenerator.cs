using System;
using System.Collections.Generic;
using System.Linq;

public class FieldGenerator
{
    private List<bool> _cells = new List<bool>();
    public FieldGenerator(int n, int m, int bombsNum)
        : this(n, m, bombsNum, new List<int>()) { }
    public FieldGenerator(int n, int m, int bombsNum, List<int> excludedIndices)
    {
        Random rand = new Random();
        int totalCells = n * m;

        // Все возможные индексы, кроме исключённых
        List<int> availableIndices = Enumerable.Range(0, totalCells)
            .Except(excludedIndices)
            .ToList();

        if (bombsNum > availableIndices.Count)
            throw new ArgumentException("Слишком много мин для заданных исключений");

        // Перемешиваем доступные индексы (Fisher-Yates)
        for (int i = availableIndices.Count - 1; i > 0; i--)
        {
            int j = rand.Next(i + 1);
            (availableIndices[i], availableIndices[j]) = (availableIndices[j], availableIndices[i]);
        }

        int[] bombIndices = availableIndices.Take(bombsNum).ToArray();

        bool[] doneCells = new bool[totalCells];
        foreach (int idx in bombIndices)
            doneCells[idx] = true;

        _cells = doneCells.ToList();
    }

    public List<bool> GetField() => _cells;
}