using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

using ChessEngine;
using ASmirnov;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenu : MonoBehaviour
{
    public static MainMenu Singleton { get; private set; }
    public ChessGameType GameType { get; set; }

    public Button classicMatch_button;
    public Button losAlamosMatch_button;
    public Button chaturngaMatch_button;
    public Button circledMatch_button;

    public Toggle playVersusComputer_toggle;

    public Text choosePlayr_text;
    public Toggle whitePlayer_toggle;
    public Toggle blackPlayer_toggle;

    public Text gameDescription_text;
    public Animator infoPanel_anim;

    public Text console_text;
    [HideInInspector]
    public string consoleText_newVal;
    private bool doSetConsoleText;
    private bool doAppendConsoleText;

    public FENConstants.ClassicFEN classicFEN;
    public FENConstants.LosAlamosFEN losAlamosFEN;
    public FENConstants.ChaturangaFEN chaturangaFEN;
    public FENConstants.CircledFEN circledFEN;

    private IBoardInitializer boardInitializer;

    public IMatchController matchController;

    private int call_trigger;
    private int back_trigger;
    private bool playVersusComputer;
    private ChessGameType gameType;

    public Animator savedGamesPanel_animator;
    public Transform savedGamesContentPanel_transform;
    private List<SavedGameMenuItem> savedGameMenuItems = new List<SavedGameMenuItem>();


    private void Start()
    {
        Singleton = this;
        call_trigger = Animator.StringToHash("call");
        back_trigger = Animator.StringToHash("back");
        playVersusComputer_toggle.isOn = false;

        LoadSavedGamesList();
        StartCoroutine(ChooseMatchWithDelay(0.1f));
    }

    private IEnumerator ChooseMatchWithDelay(float seconds)
    {
        float delta = 0f;
        while (delta < seconds)
        {
            delta += Time.deltaTime;
            yield return null;
        }

        ChooseLosAlamosMatch();
    }

    private void Update()
    {
        if (doSetConsoleText)
        {
            doSetConsoleText = false;
            console_text.text = consoleText_newVal;
        }
        if (doAppendConsoleText)
        {
            doAppendConsoleText = false;
            console_text.text += ("; " + consoleText_newVal);
        }
    }

    public void SetConsoleText(string text)
    {
        consoleText_newVal = text;
        doSetConsoleText = true;
    }

    public void AppendConsoleText(string text)
    {
        consoleText_newVal = text;
        doAppendConsoleText = true;
    }

    public BoardPrefabs GetCurrentBoardPrefabs()
    {
        switch (GameType)
        {
            case ChessGameType.classic:
                return MainPreload.Classic_board_and_players.boardPrefabs;
            case ChessGameType.los_alamos:
                return MainPreload.Los_alamos_board_and_players.boardPrefabs;
            case ChessGameType.chaturanga:
                return MainPreload.Chaturanga_board_and_players.boardPrefabs;
            case ChessGameType.circled:
                return MainPreload.Circled_board_and_players.boardPrefabs;
            default:
                throw new System.NotImplementedException("GetCurrentBoardPrefabs Not Implemented Completely!");
        }
    }

    public void SetMoveTextValue(Vector2Int prev_pos, Vector2Int new_pos)
    {
        var sb = new StringBuilder();
        switch (matchController.ChessMatchCurrentState.PreviousPlayer.Side)
        {
            case ChessSide.white:
                sb.Append("White player moves: ");
                break;
            case ChessSide.black:
                sb.Append("Black player moves: ");
                break;
            case ChessSide.red:
                sb.Append("Red player moves: ");
                break;
            case ChessSide.green:
                sb.Append("Green player moves: ");
                break;
            default:
                throw new System.NotImplementedException("Set Move Text Value Not Implemented Completely!");
        }

        sb.Append(UciConverter.BoardPositionToString(prev_pos.ToBoardPosition()) + UciConverter.BoardPositionToString(new_pos.ToBoardPosition()));
        SetConsoleText(sb.ToString());
    }


    public void InitStatics()
    {
#if UNITY_EDITOR
        DestroyImmediate(GameObject.Find(ChessConstants.BoardItemsObjectName));
        DestroyImmediate(GameObject.Find(ChessConstants.ChessItemsObjectName));
        DestroyImmediate(GameObject.Find(ChessConstants.BoardObjectName));
#else
        Destroy(GameObject.Find(ChessConstants.BoardItemsObjectName));
        Destroy(GameObject.Find(ChessConstants.ChessItemsObjectName));
        Destroy(GameObject.Find(ChessConstants.BoardObjectName));
#endif

        var board = new GameObject(ChessConstants.BoardObjectName);
        var boardItemsParent = new GameObject(ChessConstants.BoardItemsObjectName);
        var chessItemsParent = new GameObject(ChessConstants.ChessItemsObjectName);

        boardItemsParent.transform.SetParent(board.transform);
        chessItemsParent.transform.SetParent(board.transform);
    }

    public void PlayVersusComputer(bool value)
    {
        Debug.Log("Play Versus Computer!");
        choosePlayr_text.gameObject.SetActive(value);
        whitePlayer_toggle.gameObject.SetActive(value);
        blackPlayer_toggle.gameObject.SetActive(value);
        SetClassicPlayerProperies();
    }

    private void SetClassicPlayerProperies()
    {
        if (playVersusComputer_toggle.isOn)
        {
            if (whitePlayer_toggle.isOn)
            {
                MainPreload.Classic_board_and_players.playerProterties[0].type = PlayerType.human;
                MainPreload.Classic_board_and_players.playerProterties[1].type = PlayerType.computer;
            }
            else
            {
                MainPreload.Classic_board_and_players.playerProterties[0].type = PlayerType.computer;
                MainPreload.Classic_board_and_players.playerProterties[1].type = PlayerType.human;
            }
        }
        else
        {
            MainPreload.Classic_board_and_players.playerProterties[0].type = PlayerType.human;
            MainPreload.Classic_board_and_players.playerProterties[1].type = PlayerType.human;
        }
    }

    public void SetWhitePlayer(bool value)
    {
        Debug.Log("Set White Player!");
        SetClassicPlayerProperies();
    }

    public void SetBlackPlayer(bool value)
    {
        Debug.Log("Set Black Player!");
        SetClassicPlayerProperies();
    }

    public void ShowInfoPanel()
    {
        infoPanel_anim.gameObject.SetActive(true);
        infoPanel_anim.SetTrigger(call_trigger);
    }

    public void HideInfoPanel()
    {
        infoPanel_anim.SetTrigger(back_trigger);
        infoPanel_anim.gameObject.SetActive(false);
    }

    public void BackMove()
    {
        matchController.MoveBack.MoveBack();
    }

    public void StartMatch()
    {
        var sb = new StringBuilder();
        switch (gameType)
        {
            case ChessGameType.classic:
                sb.Append("Classic ");
                //ChooseClassicMatch();
                break;
            case ChessGameType.los_alamos:
                sb.Append("Los Alamos ");
                //ChooseLosAlamosMatch();
                break;
            case ChessGameType.chaturanga:
                sb.Append("Chaturanga ");
                //ChooseChaturangaMatch();
                break;
            case ChessGameType.circled:
                sb.Append("Circled ");
               // ChooseCircledMatch();
                break;
            default:
                throw new System.NotImplementedException("StartMatch func Not Implemented Completely!");
        }
        sb.Append("match started!");
        SetConsoleText(sb.ToString());

        matchController.MatchView_ControllerAPI.StartMatch();
    }

    public void ChooseClassicMatch()
    {
        SetClassicMatchMenu();
        StartClassicMatch();
    }

    public void SetClassicMatchMenu()
    {
        playVersusComputer_toggle.gameObject.SetActive(true);
        if (playVersusComputer_toggle.isOn)
        {
            choosePlayr_text.gameObject.SetActive(true);
            whitePlayer_toggle.gameObject.SetActive(true);
            blackPlayer_toggle.gameObject.SetActive(true);
        }

        classicMatch_button.GetComponent<Animator>().SetTrigger(call_trigger);
        losAlamosMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        chaturngaMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        circledMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);

        gameDescription_text.text = ChessConstants.ClassicGameDescription;
        gameType = ChessGameType.classic;
    }

    public void ChooseLosAlamosMatch()
    {
        SetLosAlamosMatchMenu();
        StartLosAlamosMatch();
    }
    public void SetLosAlamosMatchMenu()
    {
        playVersusComputer_toggle.gameObject.SetActive(false);
        choosePlayr_text.gameObject.SetActive(false);
        whitePlayer_toggle.gameObject.SetActive(false);
        blackPlayer_toggle.gameObject.SetActive(false);

        classicMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        losAlamosMatch_button.GetComponent<Animator>().SetTrigger(call_trigger);
        chaturngaMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        circledMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);

        gameDescription_text.text = ChessConstants.LosAlamosGameDescription;
        gameType = ChessGameType.los_alamos;
    }

    public void ChooseChaturangaMatch()
    {
        SetChaturangaMatchMenu();
        StartChaturangaMatch();
    }

    public void SetChaturangaMatchMenu()
    {
        playVersusComputer_toggle.gameObject.SetActive(false);
        choosePlayr_text.gameObject.SetActive(false);
        whitePlayer_toggle.gameObject.SetActive(false);
        blackPlayer_toggle.gameObject.SetActive(false);

        classicMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        losAlamosMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        chaturngaMatch_button.GetComponent<Animator>().SetTrigger(call_trigger);
        circledMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);

        gameDescription_text.text = ChessConstants.ChaturangaGameDescription;
        gameType = ChessGameType.chaturanga;
    }

    public void ChooseCircledMatch()
    {
        SetCircledMatchMenu();
        StartCircledMatch();
    }

    public void SetCircledMatchMenu()
    {
        playVersusComputer_toggle.gameObject.SetActive(false);
        choosePlayr_text.gameObject.SetActive(false);
        whitePlayer_toggle.gameObject.SetActive(false);
        blackPlayer_toggle.gameObject.SetActive(false);

        classicMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        losAlamosMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        chaturngaMatch_button.GetComponent<Animator>().SetTrigger(back_trigger);
        circledMatch_button.GetComponent<Animator>().SetTrigger(call_trigger);

        gameDescription_text.text = ChessConstants.CircledGameDescription;
        gameType = ChessGameType.circled;
    }

    private GameObject Init_()
    {
        InitStatics();
        UnityCustoms.DestroyGameObjects(ChessConstants.MatchViewObjectName);
        return new GameObject(ChessConstants.MatchViewObjectName);
    }

    public void StartClassicMatch()
    {
        var gameObj_MatchView = Init_();
        boardInitializer = gameObj_MatchView.AddComponent<ClassicBoardInitializer>();
        matchController = boardInitializer.InitializeBoard();
    }

    public void StartLosAlamosMatch()
    {
        var gameObj_MatchView = Init_();
        boardInitializer = gameObj_MatchView.AddComponent<LosAlamosBoardInitializer>();
        matchController = boardInitializer.InitializeBoard();
    }

    public void StartChaturangaMatch()
    {
        var gameObj_MatchView = Init_();
        boardInitializer = gameObj_MatchView.AddComponent<ChaturangaBoardInitializer>();
        matchController = boardInitializer.InitializeBoard();
    }

    public void StartCircledMatch()
    {
        var gameObj_MatchView = Init_();
        boardInitializer = gameObj_MatchView.AddComponent<CircledBoardInitializer>();
        matchController = boardInitializer.InitializeBoard();
    }



    public void StartClassicMatch(string s, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        InitStatics();

        GameObject gameObj_MatchView = GameObject.Find(ChessConstants.MatchViewObjectName);
        if (gameObj_MatchView == null)
            gameObj_MatchView = new GameObject(ChessConstants.MatchViewObjectName);
        boardInitializer = gameObj_MatchView.AddComponent<ClassicBoardInitializer>();
        matchController = boardInitializer.InitializeBoard(s, fromPositions, toPositions);
    }

    public void StartLosAlamosMatch(string s, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        InitStatics();
        var gameObj_MatchView = GameObject.Find(ChessConstants.MatchViewObjectName);
        if (gameObj_MatchView == null)
            gameObj_MatchView = new GameObject(ChessConstants.MatchViewObjectName);
        boardInitializer = gameObj_MatchView.AddComponent<LosAlamosBoardInitializer>();
        matchController = boardInitializer.InitializeBoard(s, fromPositions, toPositions);
    }

    public void StartChaturangaMatch(string s, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        InitStatics();
        var gameObj_MatchView = GameObject.Find(ChessConstants.MatchViewObjectName);
        if (gameObj_MatchView == null)
            gameObj_MatchView = new GameObject(ChessConstants.MatchViewObjectName);
        boardInitializer = gameObj_MatchView.AddComponent<ChaturangaBoardInitializer>();
        matchController = boardInitializer.InitializeBoard(s, fromPositions, toPositions);
    }

    public void StartCircledMatch(string s, List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
    {
        InitStatics();
        var gameObj_MatchView = GameObject.Find(ChessConstants.MatchViewObjectName);
        if (gameObj_MatchView == null)
            gameObj_MatchView = new GameObject(ChessConstants.MatchViewObjectName);
        boardInitializer = gameObj_MatchView.AddComponent<CircledBoardInitializer>();
        matchController = boardInitializer.InitializeBoard(s, fromPositions, toPositions);
    }

    public void SaveGame()
    {
        string filePath = Directory.GetCurrentDirectory() + "/" + ASmirnovCustoms.SaveGamesPath + "/" + gameType.ToString() + "_" + ASmirnovCustoms.DateTimeNowString() + ".txt";
        matchController.GameLoaderSaver.SaveGame(filePath);
    }

    public void LoadSavedGamesList()
    {
        string savedGamesPath = Directory.GetCurrentDirectory() + "/" + ASmirnovCustoms.SaveGamesPath;

        if (!Directory.Exists(savedGamesPath))
            Directory.CreateDirectory(savedGamesPath);
        else
        {
            var filePathes = Directory.GetFiles(savedGamesPath, "*.txt");
            foreach (var filePath in filePathes)
            {
                var filePath1 = filePath.Replace("\\", "/");
                bool wasLoaded = false;
                foreach (var saved_item in savedGameMenuItems)
                {
                    if (saved_item.FilePath == filePath1)
                    {
                        wasLoaded = true;
                        break;
                    }
                }

                if (!wasLoaded)
                {
                    var new_object = Instantiate(MainPreload.Saved_game_menu_item_object) as GameObject;
                    var new_saved_game_item = new_object.GetComponent<SavedGameMenuItem>();
                    new_saved_game_item.InitItem(filePath1);
                    new_object.transform.SetParent(savedGamesContentPanel_transform);
                    savedGameMenuItems.Add(new_saved_game_item);

                }
               
            }
        }

    }

    public void SavedGamesPanel_Call()
    {
        LoadSavedGamesList();
        savedGamesPanel_animator.SetTrigger(call_trigger);
    }

    public void SavedGamesPanel_Back()
    {
        savedGamesPanel_animator.SetTrigger(back_trigger);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(MainMenu))]
public class MainMenuEditor : Editor
{
    private MainMenu o;

    private void OnEnable()
    {
        o = target as MainMenu;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Classic Buttons
        GUILayout.Label("Classic Match:");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Classic"))
        {
            o.ChooseClassicMatch();
            o.StartMatch();
        }
        else if (GUILayout.Button("By enum"))
        {
            o.ChooseClassicMatch();
            o.StartClassicMatch(FENConstants.GetClassicFEN(o.classicFEN), new List<BoardPosition>(), new List<BoardPosition>());
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        GUILayout.EndHorizontal();
        //Los Alamos Buttobs
        GUILayout.Label("Los Alamos Match:");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Classic"))
        {
            o.ChooseLosAlamosMatch();
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        else if (GUILayout.Button("By enum"))
        {
            o.ChooseLosAlamosMatch();
            o.StartLosAlamosMatch(FENConstants.GetLosAlamosFEN(o.losAlamosFEN), new List<BoardPosition>(), new List<BoardPosition>());
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        GUILayout.EndHorizontal();
        //Chaturanga Buttons
        GUILayout.Label("Chaturanga Match:");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Classic"))
        {
            o.ChooseChaturangaMatch();
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        else if (GUILayout.Button("By enum"))
        {
            o.ChooseChaturangaMatch();
            o.StartChaturangaMatch(FENConstants.GetChaturangaFEN(o.chaturangaFEN), new List<BoardPosition>(), new List<BoardPosition>());
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        GUILayout.EndHorizontal();
        //Circled Buttons
        GUILayout.Label("Circled Match:");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Classic"))
        {
            o.ChooseCircledMatch();
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        else if (GUILayout.Button("By enum"))
        {
            o.ChooseCircledMatch();
            o.StartCircledMatch(FENConstants.GetCircledFEN(o.circledFEN), new List<BoardPosition>(), new List<BoardPosition>());
            o.SetConsoleText("");
            o.matchController.MatchView_ControllerAPI.StartMatch();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        if (o.matchController != null)
            GUILayout.Label("Current player: " + o.matchController.ChessMatchCurrentState.CurrentPlayer.Side.ToString());
        else
            GUILayout.Label("Current player: NOT SET");

        if (o.matchController == null)
            return;


        for (int j = o.matchController.FigureOnBoard.BoardSizeY - 1; j >= 0; j--)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < o.matchController.FigureOnBoard.BoardSizeX; i++)
            {
                var item = o.matchController.FigureOnBoard.GetFigureByPosition(new BoardPosition(i, j));
                GUILayout.TextField(UciConverter.GetFigureLetter(item.IsNullObject, item.Side, item.Type));
            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif

