using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class BoardPrefabs
{
    [Header("Board item prefabs")]
    public GameObject bright_board_item;
    public GameObject dark_board_item;
    [Header("Chess prefabs")]
    public List<ChessPrefabsPack> chessPackList;
}

[System.Serializable]
public struct ChessPrefabsPack
{
    public GameObject pawn;
    public GameObject rook;
    public GameObject bishop;
    public GameObject knight;
    public GameObject queen;
    public GameObject king;
}


