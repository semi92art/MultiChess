using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ASmirnov;
using ChessEngine;

public sealed class ChessItemMono : ChessItemMonoBase
{
    protected override void Start()
    {
        base.Start();

        switch (MainMenu.Singleton.GameType)
        {
            case ChessGameType.classic:
                matchView = FindObjectOfType<ClassicChessMatchView>() as IMatchView_Internal;
                break;
            case ChessGameType.los_alamos:
                matchView = FindObjectOfType<LosAlamosMatchView>() as IMatchView_Internal;
                break;
            case ChessGameType.chaturanga:
                matchView = FindObjectOfType<ChaturangaMatchView>() as IMatchView_Internal;
                break;
            default:
                throw new System.NotImplementedException("Start Function Was Not Implemented Completely!");
        }

        if (matchView == null)
            throw new System.NullReferenceException(MainMenu.Singleton.GameType.ToString() + " Match View Not Found!");
    }
}

