using UnityEngine;
using System.Collections.Generic;
using ChessEngine;


public sealed class LosAlamosBoardInitializer : BoardInitializerBase
{
    public override IMatchController InitializeBoard()
    {
        return Init_Board(ChessEngineConstants.FEN_LosAlamos_Default, new List<BoardPosition>(), new List<BoardPosition>());
    }

    public override IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        return Init_Board(fen, fromPositions, toPositions);
    }

    protected override IMatchController Init_Board(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        if (MainPreload.Classic_board_and_players.playerProterties.Length == 0)
            throw new System.NullReferenceException("LosAlamos Player Properties List Was Not Initialized!");

        if (ServiceLocator.Singleton == null)
        {
            var serviceLocatorFactory = new ServiceLocatorFactory();
            serviceLocatorFactory.CreateServiceLocator(GameLoaderSaverType.FEN, ChessEngineType.Stockfish, ASmirnov.ASmirnovCustoms.SaveGamesPath);
        }

        MainMenu.Singleton.GameType = ChessGameType.los_alamos;

        playerFactory = new PlayerFactory();
        
        players = new IPlayer[MainPreload.Los_alamos_board_and_players.playerProterties.Length];
        int k = -1;
        foreach (var item in MainPreload.Los_alamos_board_and_players.playerProterties)
        {
            k++;
            players[k] = playerFactory.CreateHumanPlayer(item.side);
        }

        var c_factory = new ControllerFactory();
        var controller = c_factory.CreateLosAlamosMatchController(fen, fromPositions, toPositions, players[0], players[1]);

        return controller;
    }
}
