using UnityEngine;
using System.Collections.Generic;
public class Starter : MonoBehaviour
{
    [SerializeField] private Saver _saver;
    [SerializeField] private FieldHolder _holder;
    
    private static List<bool> _field = null;
    private static List<int> _fieldSprites = null;
    private void Start()
    {
        _field = _holder.GetField();
    }
    public static List<int> GetSpritesField()
    {
        List<int> toGetList = new List<int>();
        if (_fieldSprites == null) return null;

        foreach (var fs in _fieldSprites)
        {
            toGetList.Add(fs);
        }

        return toGetList;
    }
    public static bool[,] GetTwoDimensionalField(int n, int m)
    {
        bool[,] toReturn = new bool[n, m];
        int k = 0;
        for(int i = 0; i < n; ++i)
        {
            for(int j = 0; j < m; ++j, ++k)
            {
                toReturn[i, j] = _field[k];
            }
        }

        return toReturn;
    }
}
