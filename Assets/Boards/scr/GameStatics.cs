using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using ChessEngine;

/*public class GameStatics : MonoBehaviour
{
    public static GameStatics o { get; private set; }

    public GameObject board;
    public GameObject boardItemsParent;
    public GameObject chessItemsParent;
    public Camera mainCam;
    public BoardPrefabs selectedBoard;
    public ChessSide player1_side;
    public ChessSide player2_side;
    public PlayerType player1_type;
    public PlayerType player2_type;
    public SkillLevel computer_skill;
    public ChessType currentChessType;
    public TimerType currentTimerType;
    public MatchStage matchStage;

    public IChessMatchCurrentState matchM;
    public IMatchView matchV;
    public ClassicMatchController matchC;

    private void Start()
    {
        o = this;
    }

    public void InitBoard()
    {
        DestroyImmediate(GameObject.Find("MatchV"));

        var mp = FindObjectOfType<MainPreload>();
        selectedBoard = mp.classic_board;

        IPlayer player1;
        if (player1_type == PlayerType.computer)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            player1 = new ComputerPlayer(player1_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockFishDllService());
#elif UNITY_ANDROID
            player1 = new ComputerPlayer(player1_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockfishJNIService());
#else
            player1 = new ComputerPlayer(player1_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockFishDllService());
#endif
        }
        else
            player1 = new HumanPlayer(player1_side);

        IPlayer player2;
        if (player2_type == PlayerType.computer)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            player2 = new ComputerPlayer(player2_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockFishDllService());
#elif UNITY_ANDROID
            player2 = new ComputerPlayer(player2_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockfishJNIService());
#else
            player2 = new ComputerPlayer(player2_side, int.Parse(computer_skill.ToString().Replace("level_", "")), new StockFishDllService());
#endif
        }
        else
            player2 = new HumanPlayer(player2_side);

        IPlayer whitePlayer = player1_side == ChessSide.white ? player1 : player2;
        IPlayer blackPlayer = player1_side == ChessSide.white ? player2 : player1;

        switch (currentChessType)
        {
            case ChessType.classic:
                var c_factory = new ControllerFactory();
                var controller = c_factory.CreateClassicMatchController("rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - - 0 0)", new List<FigureMove>(), whitePlayer, blackPlayer);
                break;
        }
    }
}*/

