using System.Collections;
using UnityEngine;

using Project.Piece;

class Board : MonoBehaviour
{
	public Vector3 rotationPoint;
	private float previousTime;
	public const float DEFAULT_FALL_STEP_TIME = 0.8f;
	private const int width = 10, height = 20;

	// private List<Row> board  = new List<Row>(height);
	private PieceQueue queue;
	private PieceController controller = new PieceController();


	private void LockPiece() {
		
	}

	private void ClearFullRows() {
		int clearedCount = 0;
		int y = 0;

		while (y < height) {
			if (Board.rows[y].IsFull) {
				Board.rows.RemoveAt(y);
				Board.rows.Add(new Row());
				clearedCount++;
			}
			else
				y--;
		}

		if (clearedCount > 0)
			UpdateScoreAndLevel(clearedCount);
	}

	private class PieceController
	{
		string name;
		int rotation;
		(int x, int y) position;
		bool isGrounded;

		PieceController(string startingPiece)
		{
			name = startingPiece;
			rotation = 0;
			// position = getSpawnPoint();
			isGrounded = false;
		}

		// Spawn next piece in queue
		void SpawnPiece() {}

		// Swap current piece with the held piece
		void SwapPiece() {}

		// Move piece horizontally
		void MovePiece(int step) 
		{
			transform.position += new Vector3(step, 0, 0);
			if (!ValidMove()) { transform.position -= new Vector3(step, 0, 0); }
		}

		// Rotate piece
		void RotatePiece(int factor) 
		{
			transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90 * factor);
			if (!ValidMove())
			{
				transform.position += new Vector3(1, 0, 0);
				//Checking all possible valid rotation positions
				if (!ValidMove()) { transform.position += new Vector3(-2, 0, 0); }
				if (!ValidMove()) { transform.position += new Vector3(1, 1, 0); }
				if (!ValidMove()) { transform.position += new Vector3(1, 0, 0); }
				if (!ValidMove()) { transform.position += new Vector3(-2, 0, 0); }
				if (!ValidMove())
				{
					transform.position += new Vector3(1, -1, 0);
					transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90 * factor);
				}
			}
		}

		// Move the piece down as far as possible and set the piece
		void HardDrop() 
		{
			while (ValidMove())
				transform.position += new Vector3(0, -1, 0);

			if (!ValidMove()) { transform.position += new Vector3(0, 1, 0); }
		}

		// Move the piece down faster
		void SoftDrop() 
		{
			DescendPiece(DEFAULT_FALL_STEP_TIME / 10f);
		}

		// Move the piece down 1 cell
		void ApplyGravity() 
		{
			DescendPiece(DEFAULT_FALL_STEP_TIME);
		}
		
		void DescendPiece(int fallStepTime) 
		{
			if (Time.time - previousTime > fallStepTime)
			{
				transform.position += new Vector3(0, -1, 0);
				if (!ValidMove()) { transform.position -= new Vector3(0, -1, 0); }
				previousTime = Time.time;
			}
		}

		private bool ValidMove()
		{
			foreach (Transform children in transform)
			{
				int roundedX = Mathf.RoundToInt(children.transform.position.x);
				int roundedY = Mathf.RoundToInt(children.transform.position.y);

				if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
					 return false;
			}

        return true;
		}

		private void LockPiece() {
			
		}
	}

	private (bool, int, int) ValidateRotation(int pre, int post)
	{
		// get target rotation state data
		int[,] postData = PieceRotationStateData<I>[post];
		// lookup kick table		
		(int, int)[] kickData = Rotation.GetKickData(current.name, pre, post);

		int validCase = -1, offsetX = 0, offsetY = 0;
		for (int i = 0; i < 5 && validCase <= 0; i++)
		{
			(offsetX, offsetY) = kickData[validCase];
			if (!CollisionWithOffset(postData, offsetX, offsetY))
				validCase = i;
		}

		return (validCase != -1, offsetX, offsetY);
	}


	private bool CollisionWithOffset(int[,] data, int x, int y)
	{
		int candX = current.posX + X;
		int candY = current.posY + Y;

		// check if cells of rotation must be filled while the corresponding board cell is filled
		int collided = false;
		for (int i = 0; i < 4 && !collided; i++)
			for (int j = 0; j < 4 && !collided; j++)
				if (data[i][j] == 1 && board.cells[i + candX][j + candY] == 1)
					collided = true;

		return !collided;
	}

}
