namespace Project.Piece {

	enum PieceName {
		O, I, T, Z, S, L, J
	}

	static class PieceRotation {

		enum State {
			NORTH = 0,
			EAST = 1,
			SOUTH = 2,
			WEST = 3,
		}	

		static readonly int[,,] StateData = {
			{ // O-Piece
				{
					{0,0,0,0},
					{0,1,1,0},
					{0,1,1,0},
					{0,0,0,0}	
				},
				{
					{0,0,0,0},
					{0,1,1,0},
					{0,1,1,0},
					{0,0,0,0}	
				},
				{
					{0,0,0,0},
					{0,1,1,0},
					{0,1,1,0},
					{0,0,0,0}	
				},
				{
					{0,0,0,0},
					{0,1,1,0},
					{0,1,1,0},
					{0,0,0,0}	
				}
			},
			{ // I-Piece
				{
					{0,0,0,0},
					{1,1,1,1},
					{0,0,0,0},
					{0,0,0,0} 
				},
				{	
					{0,0,1,0},
					{0,0,1,0},
					{0,0,1,0},
					{0,0,1,0} 
				},
				{
					{0,0,0,0},
					{0,0,0,0},
					{1,1,1,1},
					{0,0,0,0} 
				},
				{
					{0,1,0,0},
					{0,1,0,0},
					{0,1,0,0},
					{0,1,0,0} 
				},
			},
			{	// T-Piece
				{
					{0,1,0},
					{1,1,1},
					{0,0,0} 
				},
				{
					{0,1,0},
					{0,1,1},
					{0,1,0} 
				},
				{
					{0,0,0},
					{1,1,1},
					{0,1,0} 
				},
				{
					{0,1,0},
					{1,1,0},
					{0,1,0} 
				},
				
			},
			{ // Z-Piece
				{
					{1,1,0},
					{0,1,1},
					{0,0,0} 
				},
				{
					{0,0,1},
					{0,1,1},
					{0,1,0} 
				},
				{
					{0,0,0},
					{1,1,0},
					{0,1,1}
				},
				{
					{0,1,0},
					{1,1,0},
					{1,0,0} 
				},
			},
			{ // S-Piece
				{
					{0,1,1},
					{1,1,0},
					{0,0,0} 
				},
				{
					{0,1,0},
					{0,1,1},
					{0,0,1} 
				},
				{
					{0,0,0},
					{0,1,1},
					{1,1,0}
				},
				{
					{1,0,0},
					{1,1,0},
					{0,1,0} 
				},
			}, 
			{ // L-Piece
				{
					{0,0,1},
					{1,1,1},
					{0,0,0} 
				},
				{
					{0,1,0},
					{0,1,0},
					{0,1,1} 
				},
				{
					{0,0,0},
					{1,1,1},
					{1,0,0} 
				},
				{
					{1,1,0},
					{0,1,0},
					{0,1,0} 
				},
			}, 
			{	// J-Piece
				{
					{1,0,0},
					{1,1,1},
					{0,0,0} 
				},
				{
					{0,1,1},
					{0,1,0},
					{0,1,0} 
				},
				{
					{0,0,0},
					{1,1,1},
					{0,0,1} 
				},
				{
					{0,1,0},
					{0,1,0},
					{1,1,0} 
				}
			},
		};
		
		enum RotationType {
			NE,EN,ES,SE,SW,WS,WN,NW
		}

		static readonly (int,int)[,] KickData = // test for 5 posible kick position
		{
			{ ( 0, 0), (-1, 0), (-1, 1), ( 0,-2), (-1,-2) }, 
			{ ( 0, 0), (+1, 0), (+1,-1), ( 0,+2), (+1,+2) },
			{ ( 0, 0), (+1, 0), (+1,-1), ( 0,+2), (+1,+2) },
			{ ( 0, 0), (-1, 0), (-1,+1), ( 0,-2), (-1,-2) },
			{ ( 0, 0), (+1, 0), (+1,+1), ( 0,-2), (+1,-2) },
			{ ( 0, 0), (-1, 0), (-1,-1), ( 0,+2), (-1,+2) },
			{ ( 0, 0), (-1, 0), (-1,-1), ( 0,+2), (-1,+2) },
			{ ( 0, 0), (+1, 0), (+1,+1), ( 0,-2), (+1,-2) }
		};

		static readonly (int,int)[,] KickDataI = 
		{
			{ ( 0, 0), (-2, 0), (+1, 0), (-2,-1), (+1,+2) },
			{ ( 0, 0), (+2, 0), (-1, 0), (+2,+1), (-1,-2) },
			{ ( 0, 0), (-1, 0), (+2, 0), (-1,+2), (+2,-1) },
			{ ( 0, 0), (+1, 0), (-2, 0), (+1,-2), (-2,+1) },
			{ ( 0, 0), (+2, 0), (-1, 0), (+2,+1), (-1,-2) },
			{ ( 0, 0), (-2, 0), (+1, 0), (-2,-1), (+1,+2) },
			{ ( 0, 0), (+1, 0), (-2, 0), (+1,-2), (-2,+1) },
			{ ( 0, 0), (-1, 0), (+2, 0), (-1,+2), (+2,-1) }
		};
		
		static (int, int)[] GetKickData(PieceName piece, State pre, State post) {
			var pieceKickData;

			switch (piece) {
				case O:
					return (0, 0); 
				case I:
					pieceKickData = KickDataI;
					break;
				default:
					pieceKickData = KickData;
					break;
			}


			switch ((pre, post)) {
				case (NORTH, EAST):
					return pieceKickData.NE;
				case (EAST, NORTH):
					return pieceKickData.EN;
				case (EAST, SOUTH):
					return pieceKickData.ES;
				case (SOUTH, EAST):
					return pieceKickData.SE;
				case (SOUTH, WEST):
					return pieceKickData.SW;
				case (WEST, SOUTH):
					return pieceKickData.WS;
				case (NORTH, WEST):
					return pieceKickData.WN;
				case (WEST, NORTH):
					return pieceKickData.NW;
			}
		}
	}
}
