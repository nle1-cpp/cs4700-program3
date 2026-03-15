using UnityEngine;

public sealed class Game : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private int previewCount = 7;

    private PieceQueue pieceQueue;
    private int score;
    private int totalLines;

    public bool IsGameOver { get; private set; }

    public Tetromino[] UpcomingPieces
    {
        get { return pieceQueue == null ? new Tetromino[0] : pieceQueue.Contents; }
    }

    private void Start()
    {
        if (board == null)
        {
            board = FindObjectOfType<Board>();
        }

        if (board == null)
        {
            Debug.LogError("Game could not find a Board in the scene.");
            enabled = false;
            return;
        }

        board.Initialize(this);
        StartNewGame();
    }

    private void Update()
    {
        if (IsGameOver && Input.GetKeyDown(KeyCode.R))
        {
            StartNewGame();
        }
    }

    public void StartNewGame()
    {
        IsGameOver = false;
        score = 0;
        totalLines = 0;
        pieceQueue = new PieceQueue(previewCount);
        board.ClearBoard();
        SpawnNextPiece();
    }

    public void OnPieceLocked(int clearedLines)
    {
        if (clearedLines > 0)
        {
            totalLines += clearedLines;
            score += ScoreForLines(clearedLines);
        }

        SpawnNextPiece();
    }

    private void SpawnNextPiece()
    {
        Tetromino next = pieceQueue.Pop();
        bool spawned = board.SpawnPiece(next);
        if (!spawned)
        {
            IsGameOver = true;
        }
    }

    private int ScoreForLines(int lineCount)
    {
        switch (lineCount)
        {
            case 1: return 100;
            case 2: return 300;
            case 3: return 500;
            case 4: return 800;
            default: return lineCount * 200;
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, 260f, 220f), GUI.skin.box);
        GUILayout.Label("Score: " + score);
        GUILayout.Label("Lines: " + totalLines);
        GUILayout.Space(8f);
        GUILayout.Label("Next:");

        Tetromino[] preview = UpcomingPieces;
        for (int i = 0; i < preview.Length; i++)
        {
            GUILayout.Label((i + 1) + ". " + preview[i]);
        }

        GUILayout.Space(10f);
        GUILayout.Label("Left/Right: Move");
        GUILayout.Label("Down: Soft Drop");
        GUILayout.Label("Space: Hard Drop");
        GUILayout.Label("Z / X: Rotate");

        if (IsGameOver)
        {
            GUILayout.Space(10f);
            GUILayout.Label("Game Over - Press R to Restart");
        }

        GUILayout.EndArea();
    }
}
