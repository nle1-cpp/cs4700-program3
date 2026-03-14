using System.Collections;
using UnityEngine;

using Game.Piece;

class Board : MonoBehavior {

	private int width = 10, height = 20;
	private List<Row> board  = new List<Row>(height);
	private PieceQueue queue;
	private BoardPiece current = new BoardPiece();


	// Spawn next piece in queue
	public void SpawnNext();

	// Swap current piece with the held piece
	public void SwapHold();

	// Move piece horizontally
	public void TryMovePiece(Movement dir);

	// Rotate piece
	public void TryRotatePiece(Rotation dir);

	// Move the piece down faster
	public void SoftDrop();

	// Move the piece down as far as possible and set the piece
	public void HardDrop();

	// Move the piece down 1 cell
	private void ApplyGravity() {
		
	}

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

	class ActivePiece {
		string name;
		int rotation;
		(int x, int y) position;
		bool isGrounded;

		ActivePiece(Piece target) {
			name = n;
			rotation = 0;
			position = Piece.GetSpawnPosition(n); // resolve default position of piece
			isGrounded = false;
		}
	}

	private (bool, int, int) ValidateRotation(int pre, int post) {
		// get target rotation state data
		int[,] postData = PieceRotationStateData<I>[post];
		// lookup kick table		
		(int, int)[] kickData = Rotation.GetKickData(current.name, pre, post);

		int validCase = -1, offsetX = 0, offsetY = 0;
		for (int i = 0; i < 5 && validCase <= 0; i++) {
			(offsetX, offsetY) = kickData[validCase];
			if (!CollisionWithOffset(postData, offsetX, offsetY))
				validCase = i;
		}

		return (validCase != -1, offsetX, offsetY);
	}

	private bool CollisionWithOffset(int[,] data, int x, int y) {
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
