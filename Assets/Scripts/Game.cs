using System.Collections;
using UnityEngine;

using Game.Common;
using Board;

public class Game : MonoBehavior
{
	private Board board;

	// useful later
	private int leftMoveHeldTime, rightMoveHeldTime;
	private bool softDropHeld, cwRotateHeld, ccwRotateHeld, flipRotateHeld;
	private int heldTimeThreshold;

	private var ctrl = board.controller;

	void Start()
	{
		board = new Board();
	}

	void Update()
	{
		// Gravity + Soft Drop
		if (Input.GetKey(KeyCode.DownArrow))
			ctrl.softDrop();
		else 
			ctrl.ApplyGravity();

		// Hard Drop
		if (Input.GetKeyDown(KeyCode.UpArrow))
			ctrl.HardDrop();

		// Move Left
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			ctrl.MovePiece(-1);
		// Move Right
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			ctrl.MovePiece(1);

		// Rotate Counter-Clockwise
		if (Input.GetKeyDown(KeyCode.Z))
			ctrl.RotatePiece(-1);
		//Rotate Clockwise
		else if (Input.GetKeyDown(KeyCode.X))
			ctrl.RotatePiece(1);
	}
}
