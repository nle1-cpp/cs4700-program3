using System.Collections.Generic;
using UnityEngine;

public sealed class Piece : MonoBehaviour
{
	[Header("Identity")]
	[SerializeField] private Tetromino tetromino;

	[Header("Blocks")]
	[Tooltip("Leave empty to auto-collect direct child transforms as blocks.")]
	[SerializeField] private Transform[] blocks;

	[Header("Movement")]
	[SerializeField] private float stepDelay = 0.8f;
	[SerializeField] private float softDropMultiplier = 0.1f;

	[Header("Rotation Origin")]
	[SerializeField] private Vector3 pivotOffset = Vector3.zero;

	private Board board;
	private float nextStepTime;
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
		nextStepTime = Time.time + stepDelay;
		RefreshBlockCache();
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
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			TryMove(Vector3.left);
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			TryMove(Vector3.right);
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

	private void HandleGravity()
	{
		float multiplier = Input.GetKey(KeyCode.DownArrow) ? Mathf.Max(0.01f, softDropMultiplier) : 1f;
		float currentStepDelay = stepDelay * multiplier;

		if (Time.time < nextStepTime)
		{
			return;
		}

		if (!TryMove(Vector3.down))
		{
			LockImmediately();
			return;
		}

		if (multiplier == softDropMultiplier)
		{
			FindObjectOfType<ScoreBoard>().AddScore(4);
		}

		nextStepTime = Time.time + currentStepDelay;
	}

	public bool TryMove(Vector3 worldDelta)
	{
		transform.position += worldDelta;
		SnapToGrid();

		if (board.IsValidPosition(this))
		{
			nextStepTime = Time.time + stepDelay;
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
				nextStepTime = Time.time + stepDelay;
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
