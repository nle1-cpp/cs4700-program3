using System.Collections.Generic;
using System.Collections.Concurrent;

using Project.Piece;

// Should be able to determine the next 14 pieces
// while conforming to the 7-bag selection rules

class PieceQueue 
{
	private Queue<PieceName> queue;
	private ConcurrentBag<PieceName> bag;

	PieceQueue() {
		queue = new Queue<PieceName>();
		bag = new ConcurrentBag<PieceName>();

		foreach(PieceName piece in Enum.GetValues(typeof(PieceName))) {
			bag.Add(Piece);
		}
	}

	public PieceName[] Contents {
		get {
			return queue.ToArray();
		}
	}

	public PieceName Pop() {
		PieceName next = queue.Pop();

		queue.Enqueue(bag.TryTake());

		if (bag.IsEmpty())
			foreach(PieceName piece in Enum.GetValues(typeof(PieceName))) {
				bag.add(piece);
			}

		return next;
	}
}
