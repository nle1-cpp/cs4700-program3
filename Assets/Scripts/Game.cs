using System.Collections;
using UnityEngine;

public class Program : MonoBehaviour
{
	private Board board;

	// useful later
	private int leftMoveHeldTime, rightMoveHeldTime;
	private bool softDropHeld, cwRotateHeld, ccwRotateHeld, flipRotateHeld;
	private int heldTimeThreshold;


	void Start()
	{
		board = new Board();
	}

	void Update() {
		// descend piece event logic
		if (current.name == None) 
			SpawnPiece();

		if (input.hardDropPressed) {
			OnHardDropPressed();
			return;
		}

		// Gravity + Soft Drop
		if (Input.GetKey(KeyCode.DownArrow))
			board.controller.softDrop();
		else 
			board.controller.ApplyGravity();

		// Hard Drop
		if (Input.GetKeyDown(KeyCode.UpArrow))
			board.controller.HardDrop();

		// Move Left
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			board.controller.MovePiece(-1);
		// Move Right
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			board.controller.MovePiece(1);

		// Rotate Counter-Clockwise
		if (Input.GetKeyDown(KeyCode.Z))
			board.controller.RotatePiece(-1);
		//Rotate Clockwise
		else if (Input.GetKeyDown(KeyCode.X))
			board.controller.RotatePiece(1);
	}
}
