using Cell;

class Row : MonoBehaviour
{
	const int COLS = 10;
	Cell[] cells = new Cell[COLS];

	bool IsFull()
	{
		for (int i = 0; i < COLS; i++) {
			if (this.cells[i].style = None)
				return false;
		}
		return true;
	}

	// void ClearRow()
	// {
	// 	//remove row from board
	// }

	// void ShiftRowsDown(fromY)
	// {
	// 	// Move each row down by 1, starting at fromY upward
	// 	FOR (y in fromY..height - 1) {
	// 		 FOR x in 0..width-1:
	// 			Board.grid[x, y - 1] = Board.grid[x, y];
	// 		// Clear the top row that got duplicated
	// 		FOR (x in 0..width-1)
	// 		 Board.grid[x, height - 1] = empty;
	// 	}
	// }
}
