using System;
using System.Collections.Generic;

public sealed class PieceQueue
{
    private readonly Queue<Tetromino> queue = new Queue<Tetromino>();
    private readonly List<Tetromino> bag = new List<Tetromino>(7);
    private readonly System.Random random = new System.Random();
    private readonly int previewCount;

    public PieceQueue(int previewCount)
    {
        this.previewCount = Math.Max(1, previewCount);
        FillQueue();
    }

    public Tetromino[] Contents
    {
        get { return queue.ToArray(); }
    }

    public Tetromino Pop()
    {
        if (queue.Count == 0)
        {
            FillQueue();
        }

        Tetromino next = queue.Dequeue();
        while (queue.Count < previewCount)
        {
            queue.Enqueue(TakeFromBag());
        }

        return next;
    }

    private void FillQueue()
    {
        while (queue.Count < previewCount)
        {
            queue.Enqueue(TakeFromBag());
        }
    }

    private Tetromino TakeFromBag()
    {
        if (bag.Count == 0)
        {
            RefillBag();
        }

        int index = random.Next(bag.Count);
        Tetromino chosen = bag[index];
        bag.RemoveAt(index);
        return chosen;
    }

    private void RefillBag()
    {
        bag.Clear();
        bag.Add(Tetromino.I);
        bag.Add(Tetromino.J);
        bag.Add(Tetromino.L);
        bag.Add(Tetromino.O);
        bag.Add(Tetromino.S);
        bag.Add(Tetromino.T);
        bag.Add(Tetromino.Z);
    }
}
