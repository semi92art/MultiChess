using System;
using System.Collections.Generic;
using UnityEngine;
using ChessEngine;

public static class ChessEngineExtentions
{
    public static BoardPosition ToBoardPosition(this Vector2Int v)
    {
        return new BoardPosition(v.x, v.y);
    }

    public static Vector2Int ToVector2Int(this BoardPosition v)
    {
        return new Vector2Int(v.horizontal, v.vertical);
    }

    public static Vector2Int[] ToVector2List(this BoardPosition[] v)
    {
        var result = new Vector2Int[v.Length];
        int k = -1;
        foreach (var item in v)
            result[++k] = item.ToVector2Int();
        
        return result;
    }
}

