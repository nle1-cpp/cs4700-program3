using System.Collections;
using UnityEngine;

enum Movement {
	Left,
	Right
}
 
enum Rotation {
	Clockwise,
	CounterClockwise,
	Flip
}

enum PieceName {
	I,T,Z,S,L,J,O // corresponds to data that maps shape to a 2d array
}

class Board : MonoBehavior {

	private int width = 10, height = 20;
	private Cell[,] cells = new Cell[width,height];
	private PieceName currentPiece, holdPiece;
	private Queue queue;

	// Spawn next piece in queue
	public void SpawnPiece();

	// Swap current piece with the held piece
	public void SwapPiece();

	// Move piece horizontally
	public void Move(Movement dir);

	// Rotate piece
	public void Rotate(Rotation dir);
	
	// Move the piece down faster
	public void SoftDrop();

	// Move the piece down as far as possible and set the piece
	public void HardDrop();

	// Move the piece down 1 cell
	public void ApplyGravity();

	// Handle rotation collision cases
	public Cell[,] KickOffset();
}
