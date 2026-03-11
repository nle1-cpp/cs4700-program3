using System.Collections.Generic;
using System.Collections.Concurrent;

// Should be able to determine the next 14 pieces
// while conforming to the 7-bag selection rules

class PieceQueue {
	private Queue queue;
	private ConcurrentBag bag;

	PieceQueue() {
		queue = new Queue<Piece>();
		bag = new ConcurrentBag<Piece>();

		foreach(Piece piece in Enum.GetValues(typeof(Piece))) {
			bag.Add(Piece);
		}
	}

	public Piece[] Contents {
		get {
			return queue.ToArray();
		}
	}

	public Piece Pop() {
		Piece next = queue.Pop();

		queue.Enqueue(bag.TryTake());

		if (bag.IsEmpty())
			foreach(Piece piece in Enum.GetValues(typeof(Piece))) {
				bag.add(Piece);
			}

		return next;
	}
}
