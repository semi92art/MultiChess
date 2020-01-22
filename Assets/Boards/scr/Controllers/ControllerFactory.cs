using System.Collections.Generic;
using UnityEngine;
using ChessEngine;

public class ControllerFactory
{
    public IMatchController CreateClassicMatchController(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, IPlayer whitePlayer, IPlayer blackPlayer)
    {
        if (fen == null)
            throw new System.NullReferenceException("FEN String Was Not Set!");
        if (fromPositions == null)
            throw new System.NullReferenceException("From-Positions Is Null!");
        if (toPositions == null)
            throw new System.NullReferenceException("To-Positions Is Null!");
        if (whitePlayer == null)
            throw new System.NullReferenceException("White Player Was Not Set!");
        if (blackPlayer == null)
            throw new System.NullReferenceException("Black Player Was Not Set!");

        var playerList = new List<IPlayer>();
        playerList.Add(whitePlayer);
        playerList.Add(blackPlayer);
        var model = new ClassicMatchModel(fen, fromPositions, toPositions, playerList, ServiceLocator.Singleton.GameLoaderSaverService);

        var go = GameObject.Find(ChessConstants.MatchViewObjectName);
        var view = go.AddComponent<ClassicChessMatchView>();
        var controller = new ClassicMatchController(model, model, model, model, model, model, model, view);
        view.InitObjects(model, model, model, model);
        return controller;
    }

    public IMatchController CreateLosAlamosMatchController(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, IPlayer whitePlayer, IPlayer blackPlayer)
    {
        if (fen == null)
            throw new System.NullReferenceException("FEN String Was Not Set!");
        if (fromPositions == null)
            throw new System.NullReferenceException("From-Positions Is Null!");
        if (toPositions == null)
            throw new System.NullReferenceException("To-Positions Is Null!");
        if (whitePlayer == null)
            throw new System.NullReferenceException("White Player Was Not Set!");
        if (blackPlayer == null)
            throw new System.NullReferenceException("Black Player Was Not Set!");

        var playerList = new List<IPlayer>();
        playerList.Add(whitePlayer);
        playerList.Add(blackPlayer);
        var model = new LosAlamosMatchModel(fen, fromPositions, toPositions, playerList, ServiceLocator.Singleton.GameLoaderSaverService);
        var go = GameObject.Find(ChessConstants.MatchViewObjectName);
        var view = go.AddComponent<LosAlamosMatchView>();

        var controller = new LosAlamosMatchController(model, model, model, model, model, model, view);
        view.InitObjects(model, model, model, model);
        return controller;
    }

    public IMatchController CreateChaturangaMatchController(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, IPlayer whitePlayer, IPlayer blackPlayer, IPlayer redPlayer, IPlayer greenPlayer)
    {
        if (fen == null)
            throw new System.NullReferenceException("FEN String Was Not Set!");
        if (fromPositions == null)
            throw new System.NullReferenceException("From-Positions Is Null!");
        if (toPositions == null)
            throw new System.NullReferenceException("To-Positions Is Null!");
        if (whitePlayer == null)
            throw new System.NullReferenceException("White Player Was Not Set!");
        if (blackPlayer == null)
            throw new System.NullReferenceException("Black Player Was Not Set!");
        if (redPlayer == null)
            throw new System.NullReferenceException("Red Player Was Not Set!");
        if (greenPlayer == null)
            throw new System.NullReferenceException("Green Player Was Not Set!");

        var playerList = new List<IPlayer>();
        playerList.Add(whitePlayer);
        playerList.Add(blackPlayer);
        playerList.Add(redPlayer);
        playerList.Add(greenPlayer);
        var model = new ChaturangaMatchModel(fen, fromPositions, toPositions, playerList, ServiceLocator.Singleton.GameLoaderSaverService);
        var go = GameObject.Find(ChessConstants.MatchViewObjectName);
        var view = go.AddComponent<ChaturangaMatchView>();

        var controller = new ChaturangaMatchController(model, model, model, model, model, model, view);
        view.InitObjects(model, model, model, model);
        return controller;
    }

    public IMatchController CreateCircledMatchController(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, IPlayer whitePlayer, IPlayer blackPlayer)
    {
        if (fen == null)
            throw new System.NullReferenceException("FEN String Was Not Set!");
        if (fromPositions == null)
            throw new System.NullReferenceException("From-Positions Is Null!");
        if (toPositions == null)
            throw new System.NullReferenceException("To-Positions Is Null!");
        if (whitePlayer == null)
            throw new System.NullReferenceException("White Player Was Not Set!");
        if (blackPlayer == null)
            throw new System.NullReferenceException("Black Player Was Not Set!");

        var playerList = new List<IPlayer>();
        playerList.Add(whitePlayer);
        playerList.Add(blackPlayer);
        var model = new CircledMatchModel(fen, fromPositions, toPositions, playerList, ServiceLocator.Singleton.GameLoaderSaverService);
        var go = GameObject.Find(ChessConstants.MatchViewObjectName);
        var view = go.AddComponent<CircledMatchView>();

        var controller = new CircledMatchController(model, model, model, model, model, model, view);
        view.InitObjects(model, model, model, model);
        return controller;
    }
}

