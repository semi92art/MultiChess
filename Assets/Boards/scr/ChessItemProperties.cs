using UnityEngine;
using ChessEngine;

[System.Serializable]
public struct ChessItemProperties
{
    public Vector2Int pos;
    public byte type;
    public ChessSide side;
    public bool isNullObject;

    public ChessItemProperties(Vector2Int pos, byte type, ChessSide side, bool isNullObject = false)
    {
        this.pos = pos;
        this.type = type;
        this.side = side;
        this.isNullObject = isNullObject;
    }

    public ChessItemProperties(Vector2Int pos)
    {
        this.pos = pos;
        this.type = (byte)ClassicChessItemType.pawn;
        this.side = ChessSide.white;
        this.isNullObject = true;
    }

    public bool IsNullObject()
    {
        return isNullObject;
    }
}