using UnityEngine;
using System.Collections.Generic;
using ChessEngine;


public sealed class ClassicBoardInitializer : BoardInitializerBase
{
    public override IMatchController InitializeBoard()
    {
        return Init_Board(ChessEngineConstants.FEN_Classic_Default, new List<BoardPosition>(), new List<BoardPosition>());
    }

    public override IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        return Init_Board(fen, fromPositions, toPositions);
    }

    protected override IMatchController Init_Board(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        playerFactory = new PlayerFactory();
        if (MainPreload.Classic_board_and_players.playerProterties.Length == 0)
            throw new System.NullReferenceException("Classis Player Properties List Was Not Initialized!");

        if (ServiceLocator.Singleton == null)
        {
            var serviceLocatorFactory = new ServiceLocatorFactory();
            serviceLocatorFactory.CreateServiceLocator(GameLoaderSaverType.FEN, ChessEngineType.Stockfish, ASmirnov.ASmirnovCustoms.SaveGamesPath);
        }

        MainMenu.Singleton.GameType = ChessGameType.classic;

        players = new IPlayer[MainPreload.Classic_board_and_players.playerProterties.Length];

        for (int i = 0; i < players.Length; i++)
        {
            switch (MainPreload.Classic_board_and_players.playerProterties[i].type)
            {
                case PlayerType.human:
                    players[i] = playerFactory.CreateHumanPlayer(MainPreload.Classic_board_and_players.playerProterties[i].side);
                    break;
                case PlayerType.computer:
                    players[i] = playerFactory.CreateComputerPlayer(MainPreload.Classic_board_and_players.playerProterties[i].side, MainPreload.Classic_board_and_players.playerProterties[i].skillLevel, ServiceLocator.Singleton.ChessUCIEngine);
                    break;
                default:
                    players[i] = null;
                    break;
            }
        }

        var c_factory = new ControllerFactory();
        var controller = c_factory.CreateClassicMatchController(fen, fromPositions, toPositions, players[0], players[1]);

        return controller;
    }
}

