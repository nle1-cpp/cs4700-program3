using UnityEngine;

public enum Tetromino
{
    I,
    J,
    L,
    O,
    S,
    T,
    Z
}

public static class TetrominoData
{
    private static readonly Vector2Int[][] GeneralKickTable =
    {
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) }, // 0 -> 1
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) },     // 1 -> 0
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, -1), new Vector2Int(0, 2), new Vector2Int(1, 2) },     // 1 -> 2
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(0, -2), new Vector2Int(-1, -2) }, // 2 -> 1
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2) },    // 2 -> 3
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },  // 3 -> 2
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(-1, -1), new Vector2Int(0, 2), new Vector2Int(-1, 2) },  // 3 -> 0
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(0, -2), new Vector2Int(1, -2) }     // 0 -> 3
    };

    private static readonly Vector2Int[][] IKickTable =
    {
        new[] { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) }, // 0 -> 1
        new[] { new Vector2Int(0, 0), new Vector2Int(2, 0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) },  // 1 -> 0
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) },  // 1 -> 2
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1) },  // 2 -> 1
        new[] { new Vector2Int(0, 0), new Vector2Int(2, 0), new Vector2Int(-1, 0), new Vector2Int(2, 1), new Vector2Int(-1, -2) },  // 2 -> 3
        new[] { new Vector2Int(0, 0), new Vector2Int(-2, 0), new Vector2Int(1, 0), new Vector2Int(-2, -1), new Vector2Int(1, 2) },  // 3 -> 2
        new[] { new Vector2Int(0, 0), new Vector2Int(1, 0), new Vector2Int(-2, 0), new Vector2Int(1, -2), new Vector2Int(-2, 1) },  // 3 -> 0
        new[] { new Vector2Int(0, 0), new Vector2Int(-1, 0), new Vector2Int(2, 0), new Vector2Int(-1, 2), new Vector2Int(2, -1) }   // 0 -> 3
    };

    public static Vector2Int[] GetKickData(Tetromino tetromino, int fromRotation, int toRotation)
    {
        if (tetromino == Tetromino.O)
        {
            return new[] { Vector2Int.zero };
        }

        int index = GetKickIndex(fromRotation, toRotation);
        return tetromino == Tetromino.I ? IKickTable[index] : GeneralKickTable[index];
    }

    private static int GetKickIndex(int fromRotation, int toRotation)
    {
        fromRotation = Mod4(fromRotation);
        toRotation = Mod4(toRotation);

        if (fromRotation == 0 && toRotation == 1) return 0;
        if (fromRotation == 1 && toRotation == 0) return 1;
        if (fromRotation == 1 && toRotation == 2) return 2;
        if (fromRotation == 2 && toRotation == 1) return 3;
        if (fromRotation == 2 && toRotation == 3) return 4;
        if (fromRotation == 3 && toRotation == 2) return 5;
        if (fromRotation == 3 && toRotation == 0) return 6;
        return 7; // 0 -> 3
    }

    public static int Mod4(int value)
    {
        int mod = value % 4;
        return mod < 0 ? mod + 4 : mod;
    }
}
