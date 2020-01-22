using UnityEngine;
using ChessEngine;


public class ChessItemPropertiesFactory
{
    public ChessItemProperties CreateChessItemProperties(Vector2Int pos, byte type, ChessSide side)
    {
        return new ChessItemProperties(pos, type, side);
    }

    public ChessItemProperties CreateNullChessItemProperties()
    {
        return new ChessItemProperties(Vector2Int.zero, 0, ChessSide.white, true);
    }
}

