using UnityEngine;
using System.Collections.Generic;
using ChessEngine;

public sealed class CircledBoardInitializer : BoardInitializerBase
{
    public override IMatchController InitializeBoard()
    {
        return Init_Board(ChessEngineConstants.FEN_Circled_Default, new List<BoardPosition>(), new List<BoardPosition>());
    }

    public override IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        return Init_Board(fen, fromPositions, toPositions);
    }

    protected override IMatchController Init_Board(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        if (MainPreload.Circled_board_and_players.playerProterties.Length == 0)
            throw new System.NullReferenceException("Circled Player Properties List Was Not Initialized!");

        if (ServiceLocator.Singleton == null)
        {
            var serviceLocatorFactory = new ServiceLocatorFactory();
            serviceLocatorFactory.CreateServiceLocator(GameLoaderSaverType.FEN, ChessEngineType.Stockfish, ASmirnov.ASmirnovCustoms.SaveGamesPath);
        }

        MainMenu.Singleton.GameType = ChessGameType.circled;
        playerFactory = new PlayerFactory();
        players = new IPlayer[MainPreload.Circled_board_and_players.playerProterties.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = playerFactory.CreateHumanPlayer(MainPreload.Circled_board_and_players.playerProterties[i].side);
        }

        var c_factory = new ControllerFactory();
        var controller = c_factory.CreateCircledMatchController(fen, fromPositions, toPositions, players[0], players[1]);

        return controller;
    }
}
