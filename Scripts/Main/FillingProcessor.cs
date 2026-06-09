using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

[RequireComponent(typeof(Tilemap))]
public class FillingProcessor : MonoBehaviour
{
    public static event Action<AudioSource, AudioClip, BaseExternInteractionBlock> EffectsOnClick;
    public static event Action<Vector3Int, TileBase, TileBase, Tilemap, BaseExternInteractionBlock> AnimateOnClick;
    public static event Action OnDeath;
    public static event Action OnWin;

    [SerializeField] private int n, m;

    [SerializeField] private TileBase _newTileToRight;
    [SerializeField] private TileBase _defaultTile;
    [SerializeField] private TileBase[] _tileBases;
    [SerializeField] private TileBase[] _bounds;
    [SerializeField] private TileBase[] _grasses;

    [SerializeField] private AudioClip _boundSound;
    [SerializeField] private AudioClip _grassSound;
    [SerializeField] private AudioClip _defaultSound;

    [SerializeField] private TileBase _pressedBound;
    [SerializeField] private TileBase _pressedGrass;

    [SerializeField] private FieldHolder _fieldHolder;

    private TileBase _newTileToLeft;
    private Tilemap _tilemap;
    private Mouse _mouse;
    private Camera _camera;

    private bool[,] _fieldTD;
    private bool[,] _opened;

    private AudioSource _audioSource;

    private bool _isFirstClick = true;
    private bool _isDead = false;

    private ushort _clickedCounter = 0;
    private int _bombsNum;
    private void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        _mouse = Mouse.current;
        _camera = Camera.main;
    }
    private void Start()
    {
        RefreshFieldFromHolder();
        _audioSource = AudioProcessor.audioSource;
        _opened = new bool[n, m];
        _isDead = false;
        _bombsNum = _fieldHolder.GetBombsNum();
    }

    private void RefreshFieldFromHolder()
    {
        if (_fieldHolder == null)
        {
            Debug.LogError("FieldHolder is not assigned in FillingProcessor inspector!", this);
            return;
        }

        List<bool> flatField = _fieldHolder.GetField();
        if (flatField == null || flatField.Count != n * m)
        {
            Debug.LogError("Invalid field data from FieldHolder!");
            return;
        }

        _fieldTD = new bool[n, m];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                _fieldTD[i, j] = flatField[i * m + j];
    }

    private void Update()
    {
        if (_isDead) return;

        if (_clickedCounter + _bombsNum == n * m) OnWin?.Invoke();

        if (_mouse.leftButton.wasReleasedThisFrame)
        {
            bool freezeFlag = false;

            Vector2 mouseScreenPosition = _mouse.position.ReadValue();
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));

            Vector3Int cellPositionIndex = _tilemap.WorldToCell(mouseWorldPosition);
            int i = Math.Abs(cellPositionIndex.y - 2);
            int j = cellPositionIndex.x + 6;

            if (!_tilemap.HasTile(cellPositionIndex)) return;

            TileBase originalTile = _tilemap.GetTile(cellPositionIndex);
            AudioClip clip = null;
            BaseExternInteractionBlock block = null;
            TileBase newTile = null;

            if (originalTile.name == _bounds[0].name || originalTile.name == _bounds[1].name || originalTile.name == _bounds[2].name || originalTile.name == _bounds[3].name)
            {
                clip = _boundSound;
                newTile = _pressedBound;
                block = new BoundEI();
            }
            else if (originalTile.name == _grasses[0].name || originalTile.name == _grasses[1].name || originalTile.name == _grasses[2].name || originalTile.name == _grasses[3].name || originalTile.name == _grasses[4].name || originalTile.name == _grasses[5].name || originalTile.name == _grasses[6].name || originalTile.name == _grasses[7].name || originalTile.name == _grasses[8].name)
            {
                clip = _grassSound;
                newTile = _pressedGrass;
                block = new GrassEI();
            }
            else if (originalTile.name == _tileBases[10].name || originalTile.name == _defaultTile.name)
            {
                clip = _defaultSound;
                block = new DefaultEI();
                if (!_isFirstClick) ++_clickedCounter;
            }

            if (_audioSource != null && clip != null && block != null)
                EffectsOnClick?.Invoke(_audioSource, clip, block);

            if (block != null && newTile != null && originalTile != null && _tilemap != null)
                AnimateOnClick?.Invoke(cellPositionIndex, newTile, originalTile, _tilemap, block);

            if (i < 0 || i >= n || j < 0 || j >= m) return;
            if (cellPositionIndex.y - 2 > 0) return;

            _opened[i, j] = true;

            if (_isFirstClick)
            {
                _isFirstClick = false;

                _fieldHolder.RegenerateFieldForSafeClick(i, j);
                RefreshFieldFromHolder();
                _opened = new bool[n, m];

                TileBase centerTile = GetTileByBombsCount(0);
                _tilemap.SetTile(cellPositionIndex, centerTile);
                _opened[i, j] = true;
                ++_clickedCounter;

                Queue<(int, int)> queue = new Queue<(int, int)>();
                queue.Enqueue((i, j));
                while (queue.Count > 0)
                {
                    (int ci, int cj) = queue.Dequeue();
                    for (int di = -1; di <= 1; di++)
                    {
                        for (int dj = -1; dj <= 1; dj++)
                        {
                            if (di == 0 && dj == 0) continue;
                            int ni = ci + di, nj = cj + dj;
                            if (ni < 0 || ni >= n || nj < 0 || nj >= m) continue;
                            if (_opened[ni, nj]) continue;
                            if (_fieldTD[ni, nj]) continue;

                            int nb = GetBombsNum(ni, nj);
                            TileBase neighborTile = GetTileByBombsCount(nb);
                            Vector3Int neighborPos = new Vector3Int(nj - 6, -(ni - 2), 0);
                            _tilemap.SetTile(neighborPos, neighborTile);
                            _opened[ni, nj] = true;

                            if (nb == 0)
                                queue.Enqueue((ni, nj));

                            ++_clickedCounter;
                        }
                    }
                }

                return;
            }

            if (originalTile != _newTileToRight)
            {
                if (_fieldTD[i, j])
                {
                    _newTileToLeft = _tileBases[1];
                    freezeFlag = true;
                }
                else
                {
                    int bombsCounter = GetBombsNum(i, j);
                    _newTileToLeft = GetTileByBombsCount(bombsCounter);
                }

                _tilemap.SetTile(cellPositionIndex, _newTileToLeft);
                if (freezeFlag)
                {
                    _isDead = true;
                    OnDeath?.Invoke();
                }
                if (GetBombsNum(i, j) == 0)
                {
                    Queue<(int, int)> queue = new Queue<(int, int)>();
                    queue.Enqueue((i, j));
                    while (queue.Count > 0)
                    {
                        (int ci, int cj) = queue.Dequeue();
                        for (int di = -1; di <= 1; di++)
                        {
                            for (int dj = -1; dj <= 1; dj++)
                            {
                                if (di == 0 && dj == 0) continue;
                                int ni = ci + di, nj = cj + dj;
                                if (ni < 0 || ni >= n || nj < 0 || nj >= m) continue;
                                if (_opened[ni, nj]) continue;
                                if (_fieldTD[ni, nj]) continue;

                                int nb = GetBombsNum(ni, nj);
                                TileBase neighborTile = GetTileByBombsCount(nb);
                                Vector3Int neighborPos = new Vector3Int(nj - 6, -(ni - 2), 0);
                                _tilemap.SetTile(neighborPos, neighborTile);
                                _opened[ni, nj] = true;

                                if (nb == 0)
                                    queue.Enqueue((ni, nj));

                                ++_clickedCounter;
                            }
                        }
                    }
                }
            }
        }
        else if (_mouse.rightButton.wasReleasedThisFrame)
        {
            Vector2 mouseScreenPosition = _mouse.position.ReadValue();
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 0f));
            Vector3Int cellPositionIndex = _tilemap.WorldToCell(mouseWorldPosition);

            if (_tilemap.HasTile(cellPositionIndex))
            {
                TileBase tile = _tilemap.GetTile(cellPositionIndex);
                if (tile == _newTileToRight)
                    _tilemap.SetTile(cellPositionIndex, _tileBases[10]);
                else if (tile == _tileBases[10] || tile == _defaultTile)
                    _tilemap.SetTile(cellPositionIndex, _newTileToRight);
            }
        }
    }

    private TileBase GetTileByBombsCount(int count)
    {
        return count switch
        {
            0 => _tileBases[0],
            1 => _tileBases[2],
            2 => _tileBases[3],
            3 => _tileBases[4],
            4 => _tileBases[5],
            5 => _tileBases[6],
            6 => _tileBases[7],
            7 => _tileBases[8],
            8 => _tileBases[9],
            _ => _tileBases[0]
        };
    }
    private int GetBombsNum(int i, int j)
    {
        int bombsCounter = 0;
        if (i > 0 && _fieldTD[i - 1, j]) ++bombsCounter;
        if (j > 0 && _fieldTD[i, j - 1]) ++bombsCounter;
        if (i < n - 1 && _fieldTD[i + 1, j]) ++bombsCounter;
        if (j < m - 1 && _fieldTD[i, j + 1]) ++bombsCounter;
        if (i > 0 && j > 0 && _fieldTD[i - 1, j - 1]) ++bombsCounter;
        if (i < n - 1 && j < m - 1 && _fieldTD[i + 1, j + 1]) ++bombsCounter;
        if (i > 0 && j < m - 1 && _fieldTD[i - 1, j + 1]) ++bombsCounter;
        if (i < n - 1 && j > 0 && _fieldTD[i + 1, j - 1]) ++bombsCounter;
        return bombsCounter;
    }
}