using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PiecePrefabEntry
{
    public Tetromino tetromino;
    public Piece prefab;
}

public sealed class Board : MonoBehaviour
{
    [Header("Board Size")]
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 20;

    [Header("Spawning")]
    [SerializeField] private Vector2Int spawnCell = new Vector2Int(4, 18);
    [SerializeField] private PiecePrefabEntry[] piecePrefabs;

    private Transform[,] grid;
    private readonly Dictionary<Tetromino, Piece> prefabLookup = new Dictionary<Tetromino, Piece>();
    private Game game;

    public Piece ActivePiece { get; private set; }

    public bool IsGameOver
    {
        get { return game != null && game.IsGameOver; }
    }

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    private void Awake()
    {
        grid = new Transform[width, height];
        RebuildPrefabLookup();
    }

    private void OnValidate()
    {
        width = Mathf.Max(4, width);
        height = Mathf.Max(8, height);
        RebuildPrefabLookup();
    }

    public void Initialize(Game owner)
    {
        game = owner;
        RebuildPrefabLookup();
        if (grid == null || grid.GetLength(0) != width || grid.GetLength(1) != height)
        {
            grid = new Transform[width, height];
        }
    }

    public void ClearBoard()
    {
        if (ActivePiece != null)
        {
            Destroy(ActivePiece.gameObject);
            ActivePiece = null;
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform cell = grid[x, y];
                if (cell != null)
                {
                    Destroy(cell.gameObject);
                    grid[x, y] = null;
                }
            }
        }
    }

    public bool SpawnPiece(Tetromino tetromino)
    {
        if (!prefabLookup.ContainsKey(tetromino) || prefabLookup[tetromino] == null)
        {
            Debug.LogError("No prefab assigned for tetromino " + tetromino + ".");
            return false;
        }

        if (ActivePiece != null)
        {
            Destroy(ActivePiece.gameObject);
            ActivePiece = null;
        }

        Vector3 worldSpawn = CellToWorld(spawnCell);
        Piece instance = Instantiate(prefabLookup[tetromino], worldSpawn, Quaternion.identity);
        instance.name = tetromino + " Piece";
        instance.Initialize(this);
        ActivePiece = instance;

        if (!IsValidPosition(instance))
        {
            Destroy(instance.gameObject);
            ActivePiece = null;
            return false;
        }

        return true;
    }

    public bool IsValidPosition(Piece piece)
    {
        IReadOnlyList<Transform> blocks = piece.Blocks;
        for (int i = 0; i < blocks.Count; i++)
        {
            Vector2Int cell = WorldToCell(blocks[i].position);
            if (!IsInside(cell))
            {
                return false;
            }

				if (cell.y >= height)
				{
					continue;
				}

            if (grid[cell.x, cell.y] != null)
            {
                return false;
            }
        }

        return true;
    }

    public void LockActivePiece(Piece piece)
    {
        if (piece != ActivePiece)
        {
            return;
        }

        IReadOnlyList<Transform> blocks = piece.Blocks;
        for (int i = 0; i < blocks.Count; i++)
        {
            Transform block = blocks[i];
            Vector2Int cell = WorldToCell(block.position);
            if (!IsInside(cell))
            {
                continue;
            }

            block.SetParent(transform, true);
            block.position = CellToWorld(cell);
            grid[cell.x, cell.y] = block;
        }

        Destroy(piece.gameObject);
        ActivePiece = null;

        int clearedLines = ClearFullRows();
        if (game != null)
        {
            game.OnPieceLocked(clearedLines);
        }
    }

    public Vector3 CellToWorld(Vector2Int cell)
    {
        return transform.position + new Vector3(cell.x, cell.y, 0f);
    }

    public Vector2Int WorldToCell(Vector3 worldPosition)
    {
        Vector3 local = worldPosition - transform.position;
        return new Vector2Int(Mathf.RoundToInt(local.x), Mathf.RoundToInt(local.y));
    }

    private int ClearFullRows()
    {
        int cleared = 0;

        for (int y = 0; y < height; y++)
        {
            if (!IsRowFull(y))
            {
                continue;
            }

            ClearRow(y);
            ShiftRowsDown(y + 1);
            y--;
            cleared++;
        }

        return cleared;
    }

    private bool IsRowFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    private void ClearRow(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    private void ShiftRowsDown(int startY)
    {
        for (int y = startY; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Transform block = grid[x, y];
                if (block == null)
                {
                    continue;
                }

                grid[x, y - 1] = block;
                grid[x, y] = null;
                block.position += Vector3.down;
            }
        }
    }

    private bool IsInside(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < width && cell.y >= 0;
    }

    private void RebuildPrefabLookup()
    {
        prefabLookup.Clear();
        if (piecePrefabs == null)
        {
            return;
        }

        for (int i = 0; i < piecePrefabs.Length; i++)
        {
            PiecePrefabEntry entry = piecePrefabs[i];
            prefabLookup[entry.tetromino] = entry.prefab;
        }
    }
}
