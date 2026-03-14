class Gravity {
	private int gravityTimer = 0;
	private int lockTimer = 0;
	private bool isLocking = false;

	private const int GRAVITY_STEP_SECONDS = 1;
	private const int LOCK_DELAY_SECONDS = 3;

	void Update(int deltaTime) {
		HandlePlayerInput();  // move/rotate; see lock interaction notes below

		gravityTimer += deltaTime;
		while (gravityTimer >= GRAVITY_STEP_SECONDS) {
			gravityTimer -= GRAVITY_STEP_SECONDS;

			if (TryMovePiece(0, -1)) {    // attempt to fall by 1
				isLocking = false;
				lockTimer = 0;
			}
			else {
				// can’t fall; start lock delay
				isLocking = true;
				break;
			}
		}

		if (isLocking) {
		 lockTimer += deltaTime;
			if (lockTimer >= LOCK_DELAY_SECONDS) {
				LockPiece();
				ClearLinesAndCollapse();
				SpawnNext();
				ResetTimers();
			}
		}
	}
}
