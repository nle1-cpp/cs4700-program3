using System.Collections.Generic;
using UnityEngine;

public sealed class Piece : MonoBehaviour
{
	[Header("Identity")]
	[SerializeField] private Tetromino tetromino;

	[Header("Blocks")]
	[Tooltip("Leave empty to auto-collect direct child transforms as blocks.")]
	[SerializeField] private Transform[] blocks;

	[Header("Rotation")]
	[SerializeField] private Vector3 pivotOffset = Vector3.zero;

	[Header("Movement")]
	[SerializeField] private float stepDelay = 0.8f;
	[SerializeField] private float moveRepeatDelay = 0.15f;
	[SerializeField] private float moveRepeatRate = 0.05f;
	[SerializeField] private float softDropFactor = 15f;
	[SerializeField] private float gravityFactor = 4f;

	private float horizontalHoldTime;
	private float horizontalRepeatTimer;
	private int horizontalDirection;
	private float softDropTimer;
	private float softDropInterval;

	private float gravityStepTimer;
	private float gravityStepInterval;

	private Board board;
	private bool locked;

	public Tetromino TetrominoType
	{
		get { return tetromino; }
	}

	public int RotationIndex { get; private set; }

	public IReadOnlyList<Transform> Blocks
	{
		get { return blocks; }
	}

	public void Initialize(Board ownerBoard)
	{
		board = ownerBoard;
		RotationIndex = 0;
		locked = false;
		SnapToGrid();
		gravityStepTimer = 0;
		gravityStepInterval = 1f / gravityFactor;
		RefreshBlockCache();
		softDropInterval = 1f / 60f;
	}

	private void Awake()
	{
		RefreshBlockCache();
	}

	private void OnValidate()
	{
		RefreshBlockCache();
	}

	private void Update()
	{
		if (board == null || locked || board.IsGameOver)
		{
			return;
		}

		HandleInput();
		HandleSoftDropInput();
		HandleGravity();
	}

	public void LockImmediately()
	{
		if (locked)
		{
			return;
		}

		locked = true;
		board.LockActivePiece(this);
	}

	private void HandleInput()
	{
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			HandleMovementInput(-1);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			HandleMovementInput(1);
		}
		else
		{
			HandleMovementInput(0);
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			TryRotate(-1);
		}
		else if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			TryRotate(1);
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			HardDrop();
		}
	}

	private void HandleMovementInput(int inputDir)
	{
		 if (inputDir == 0)
		 {
			  horizontalDirection = 0;
			  horizontalHoldTime = 0f;
			  horizontalRepeatTimer = 0f;
			  return;
		 }

		 if (inputDir != horizontalDirection)
		 {
			  horizontalDirection = inputDir;
			  horizontalHoldTime = 0f;
			  horizontalRepeatTimer = 0f;
			  TryMove(Vector3.right * horizontalDirection);
			  return;
		 }

		 horizontalHoldTime += Time.deltaTime;

		 if (horizontalHoldTime < moveRepeatDelay)
		 {
			  return;
		 }

		 horizontalRepeatTimer += Time.deltaTime;

		 while (horizontalRepeatTimer >= moveRepeatRate)
		 {
			  horizontalRepeatTimer -= moveRepeatRate;
			  TryMove(Vector3.right * horizontalDirection);
		 }
	}

	private void HandleSoftDropInput()
	{
		if (!Input.GetKey(KeyCode.DownArrow))
		{
			softDropTimer = 0f;
			return;
		}

		softDropTimer += Time.deltaTime;

		while (softDropTimer >= softDropInterval)
		{
			softDropTimer -= softDropInterval;

			if (!TryMove(Vector3.down))
			{
				break;
			}

			FindObjectOfType<ScoreBoard>().AddScore(4);
		}
	}


	private void HandleGravity()
	{
		gravityStepTimer += Time.deltaTime;

		if (gravityStepTimer < gravityStepInterval)
		{
			return;
		}

		gravityStepTimer = 0f;

		if (!TryMove(Vector3.down))
		{
			LockImmediately();
		}
	}

	public bool TryMove(Vector3 worldDelta)
	{
		transform.position += worldDelta;
		SnapToGrid();

		if (board.IsValidPosition(this))
		{
			return true;
		}

		transform.position -= worldDelta;
		SnapToGrid();
		return false;
	}

	public bool TryRotate(int direction)
	{
		int fromRotation = RotationIndex;
		int toRotation = TetrominoData.Mod4(RotationIndex + direction);

		float angle = direction > 0 ? -90f : 90f;
		transform.RotateAround(transform.TransformPoint(pivotOffset), Vector3.forward, angle);
		SnapToGrid();

		Vector2Int[] kicks = TetrominoData.GetKickData(tetromino, fromRotation, toRotation);
		for (int i = 0; i < kicks.Length; i++)
		{
			Vector2Int offset = kicks[i];
			transform.position += new Vector3(offset.x, offset.y, 0f);
			SnapToGrid();

			if (board.IsValidPosition(this))
			{
				RotationIndex = toRotation;
				gravityStepTimer -= stepDelay;
				return true;
			}

			transform.position -= new Vector3(offset.x, offset.y, 0f);
			SnapToGrid();
		}

		transform.Rotate(0f, 0f, -angle);
		SnapToGrid();
		return false;
	}

	public void HardDrop()
	{
		while (TryMove(Vector3.down))
		{
		}

		LockImmediately();

		FindObjectOfType<ScoreBoard>().AddScore(25);
	}

	public void SnapToGrid()
	{
		Vector3 position = transform.position;
		transform.position = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);

		RefreshBlockCache();
		for (int i = 0; i < blocks.Length; i++)
		{
			Transform block = blocks[i];
			Vector3 world = block.position;
			block.position = new Vector3(Mathf.Round(world.x), Mathf.Round(world.y), world.z);
		}
	}

	private void RefreshBlockCache()
	{
		if (blocks != null && blocks.Length > 0)
		{
			return;
		}

		List<Transform> foundBlocks = new List<Transform>();
		for (int i = 0; i < transform.childCount; i++)
		{
			foundBlocks.Add(transform.GetChild(i));
		}

		blocks = foundBlocks.ToArray();
	}
}
