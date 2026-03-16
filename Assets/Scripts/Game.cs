using UnityEngine;
using UnityEngine.SceneManagement; // Essential for scene management

public sealed class Game : MonoBehaviour
{
	[SerializeField] private Board board;
	[SerializeField] private int previewCount = 7;

	[SerializeField] private PiecePreviewDisplay holdDisplay;
	[SerializeField] private PiecePreviewDisplay queueDisplay;

	private Tetromino activePiece;
	private Tetromino heldPiece;
	private bool holdUsedThisTurn;
	private PieceQueue pieceQueue;
	private ScoreBoard scoreboard;
	private int totalLines;

	public bool IsGameOver { get; private set; }

	public Tetromino[] UpcomingPieces
	{
		get { return pieceQueue == null ? new Tetromino[0] : pieceQueue.Contents; }
	}

	void Start()
	{
		if (scoreboard == null)
		{
			scoreboard = FindObjectOfType<ScoreBoard>();

		}

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

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			StartNewGame();
		}

		if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.LeftShift))
		{
			 TryHoldPiece();
		}
	}

	public void StartNewGame()
	{
		IsGameOver = false;
		scoreboard.score = 0;
		totalLines = 0;
		pieceQueue = new PieceQueue(previewCount);
		board.ClearBoard();
		SpawnNextPiece();

		heldPiece = (Tetromino) (-1);
		holdUsedThisTurn = false;

		holdDisplay?.SetPiece((Tetromino) (-1));
		queueDisplay?.SetQueue(pieceQueue.Contents);
	}

	public void OnPieceLocked(int clearedLines)
	{
		if (clearedLines > 0)
		{
			totalLines += clearedLines;
			scoreboard.AddScore(ScoreForLines(clearedLines));
		}

		holdUsedThisTurn = false;
		SpawnNextPiece();
	}

	private void SpawnNextPiece()
	{
		activePiece = pieceQueue.Pop();
		bool spawned = board.SpawnPiece(activePiece);
		if (!spawned)
		{
			IsGameOver = true;
		}

		queueDisplay?.SetQueue(pieceQueue.Contents);
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

	public void TryHoldPiece()
	{
		 if (holdUsedThisTurn || board.ActivePiece == null)
			  return;

		 Tetromino current = board.ActivePiece.TetrominoType;
		 RemoveActivePiece();

		 if (heldPiece == Tetromino.None)
		 {
			  heldPiece = current;
			  holdUsedThisTurn = true;
			  SpawnNextPiece();
		 }
		 else
		 {
			  Tetromino swap = heldPiece;
			  heldPiece = current;
			  holdUsedThisTurn = true;
			  board.SpawnPiece(swap);
		 }

		 holdDisplay.SetPiece(heldPiece);
		 queueDisplay.SetQueue(pieceQueue.Contents);
	}

	public void RemoveActivePiece()
	{
		 if (activePiece == Tetromino.None)
			  return;

		 Destroy(board.ActivePiece.gameObject);
		 activePiece = Tetromino.None;
	}

	private void OnGUI()
	{
		// GUILayout.BeginArea(new Rect(10f, 10f, 260f, 220f), GUI.skin.box);
		// GUILayout.Label("Score: " + scoreboard.score);
		// GUILayout.Label("Lines: " + totalLines);
		// GUILayout.Space(8f);
		// GUILayout.Label("Next:");
		//
		// Tetromino[] preview = UpcomingPieces;
		// for (int i = 0; i < preview.Length; i++)
		// {
		// 	GUILayout.Label((i + 1) + ". " + preview[i]);
		// }
		//
		// GUILayout.Space(10f);
		// GUILayout.Label("Left/Right: Move");
		// GUILayout.Label("Down: Soft Drop");
		// GUILayout.Label("Space: Hard Drop");
		// GUILayout.Label("Z / X: Rotate");
		//
		// if (IsGameOver)
		// {
		// 	GUILayout.Space(10f);
		// 	GUILayout.Label("Game Over - Press R to Restart");
		// }
		//
		// GUILayout.EndArea();
	}
}
