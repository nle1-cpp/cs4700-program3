using System.Collections;
using UnityEngine;

using Game.PieceUtil;

class Board : MonoBehavior {

	private int width = 10, height = 20;
	private Cell[,] cells = new Cell[width,height];
	private Queue queue;

	private CurrentPiece current = new CurrentPiece();


	// Spawn next piece in queue
	public void SpawnPiece();

	// Swap current piece with the held piece
	public void SwapPiece();

	// Move piece horizontally
	public void MovePiece(Movement dir);

	// Rotate piece
	public void RotatePiece(Rotation dir);
	
	// Move the piece down faster
	public void SoftDrop();

	// Move the piece down as far as possible and set the piece
	public void HardDrop();

	// Move the piece down 1 cell
	public void ApplyGravity();

	class BoardPiece {
		string name;
		int rotation;
		(int x, int y) position;
		
		BoardPiece(Piece target) {
			name = n;
			rotation = 0;
			position = Util.GetDefaultPositionOfPiece(n); // resolve default position of piece
		}

		void ChangeTo(Piece target) {
			this(target);
		}
	}

	private (bool, int, int) ValidateRotation(int pre, int post) {
		// get target rotation state data
		int[,] postData = PieceRotationStateData<I>[post];
		// lookup kick table		
		(int,int)[] kickData = Rotation.GetKickData(current.name, pre, post);

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
				if (data[i][j] == 1 && board.cells[i+candX][j+candY] == 1)
					collided = true;

		return !collided;
	}
}
