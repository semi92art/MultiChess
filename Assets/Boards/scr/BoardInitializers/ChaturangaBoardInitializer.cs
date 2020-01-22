using UnityEngine;
using System.Collections.Generic;
using ChessEngine;


public sealed class ChaturangaBoardInitializer : BoardInitializerBase
{
    public override IMatchController InitializeBoard()
    {
        return Init_Board(ChessEngineConstants.FEN_Chaturanga_Default, new List<BoardPosition>(), new List<BoardPosition>());
    }

    public override IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        return Init_Board(fen, fromPositions, toPositions);
    }

    protected override IMatchController Init_Board(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        if (MainPreload.Chaturanga_board_and_players.playerProterties.Length == 0)
            throw new System.NullReferenceException("Chaturanga Player Properties List Was Not Initialized!");


        if (ServiceLocator.Singleton == null)
        {
            var serviceLocatorFactory = new ServiceLocatorFactory();
            serviceLocatorFactory.CreateServiceLocator(GameLoaderSaverType.FEN, ChessEngineType.Stockfish, ASmirnov.ASmirnovCustoms.SaveGamesPath);
        }

        MainMenu.Singleton.GameType = ChessGameType.chaturanga;

        playerFactory = new PlayerFactory();

        players = new IPlayer[MainPreload.Chaturanga_board_and_players.playerProterties.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = playerFactory.CreateHumanPlayer(MainPreload.Chaturanga_board_and_players.playerProterties[i].side);
        }

        var c_factory = new ControllerFactory();
        var controller = c_factory.CreateChaturangaMatchController(fen, fromPositions, toPositions, players[0], players[1], players[2], players[3]);

        return controller;
    }
}

