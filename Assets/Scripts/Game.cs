using System.Collections;
using UnityEngine;

using Game.Common;
using Board;

public class Game : MonoBehavior {

	private Board board;

	// useful later
	private int leftMoveHeldTime, rightMoveHeldTime;
	private bool softDropHeld, cwRotateHeld, ccwRotateHeld, flipRotateHeld;
	private int heldTimeThreshold;

	void Start() {
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

		if (input.leftPressed && TryMovePiece(-1, 0)) OnMoveOrRotateSucceeded();
		if (input.rightPressed && TryMovePiece(1, 0)) OnMoveOrRotateSucceeded();
		if (input.rotatePressed && TryRotatePiece()) OnMoveOrRotateSucceeded();
		if (input.softDropHeld && TryMovePiece(0, -1)) addSoftDropScore();

		// timer condition to decrement piece one level
		if (true) { 
			board.ApplyGravity();
		} 

		// if no keys are down skip the following ?
		//
		// handle hard dropping immediately
		if	(Input.GetButtonDown())
			board.HardDrop();

		// handle soft dropping
		if	(Input.GetButtonDown())
			board.SoftDrop();

		// handle inputs for horizontal movement
		if (Input.GetButtonDown())
			board.Move(LEFT);
		else if(Input.GetButtonDown())
			board.Move(RIGHT);

		// handle inputs for rotation
		if (Input.GetButtonDown())
			board.Rotate(CW);
		else if(Input.GetButtonDown())
			board.Rotate(CCW);
		else if(Input.GetButtonDown())
			board.Rotate(FLIP);
	}
}
