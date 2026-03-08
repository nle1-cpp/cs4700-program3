using System.Collections.Generic;

// Should be able to determine the next 14 pieces
// while conforming to the 7-bag selection rules

class PieceQueue {
	private Queue queue;
	private Bag bag;

	PieceQueue() {
		queue = new Queue<Piece>();
		bag = new Bag<Piece>();

		foreach(Piece piece in Enum.GetValues(typeof(Piece))) {
			bag.Add(Piece);
		}
	}

	public Piece[] GetNext(int amt) {
		if (amt < 14) // arbitrary lookahead limit
			throw new OutOfBoundsException();

		Piece[] next = new Piece[amt];
		// read pieces from queue to array
		return next;	
	}

	public Piece Pop() {
		Piece next = queue.Pop();

		queue.Enqueue(bag.Remove());

		if (bag.IsEmpty())
			foreach(Piece piece in Enum.GetValues(typeof(Piece))) {
				bag.add(Piece);
			}

		return next;
	}
}
