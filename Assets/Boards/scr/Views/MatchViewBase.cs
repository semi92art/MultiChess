using UnityEngine;
using ChessEngine;
using System.Collections.Generic;
using ASmirnov;
using System.Threading;
using System.Linq;

public abstract class MatchViewBase : MonoBehaviour, IMatchView_Internal, IMatchView_ControllerAPI
{
    public IChessMatchCurrentState iChessMatchCurrentState { get; protected set; }
    public IMoveBack iMoveBack { get; protected set; }
    public IChessAction iChessAction { get; protected set; }
    public IFigureOnBoard iFigureOnBoard { get; protected set; }

    
    public bool IsBoardPositionUpdated { get; protected set; }

    protected Vector3 boardPosition;
    public Vector3 BoardPositionMono
    {
        get
        {
            IsBoardPositionUpdated = false;
            return boardPosition;
        }
        protected set
        {
            IsBoardPositionUpdated = true;
            boardPosition = value;
        }
    }




    protected byte boardSizeX;
    protected byte boardSizeY;
    protected ChessItemProperties[,] board;
    protected List<BoardItemMonoBase> boardItemsMono;
    protected List<ChessItemMonoBase> chessItemsMono;

    protected ChessItemProperties currentProps;
    protected ChessItemProperties PrevProps;
    protected bool doUpdateBoardPosition;

    protected Vector2Int screenPosition;
    protected Vector2Int ScreenPosition
    {
        get { return screenPosition; }
        set
        {
            screenPosition = value;
            doUpdateBoardPosition = true;
        }
    }

    protected MainMenu mm;
    protected Vector2 boardSpriteSize;
    protected float delta_top_coeff = 1f / 12f;
    protected float delta_bottom_coeff = 4f / 12f;
    protected bool isMate, isMateChecked;
    protected bool do_action;
    protected GameObject board_items_parent;
    protected GameObject chess_items_parent;

    protected ChessItemPropertiesFactory chessItemProtertiesFactory;
    protected bool isMatchPlaying;

    protected List<BoardPosition> possibleMoveList;
    protected List<bool> isJumpList;

    protected void Start()
    {
        chessItemProtertiesFactory = new ChessItemPropertiesFactory();
        boardSpriteSize = new Vector2();
        currentProps = chessItemProtertiesFactory.CreateNullChessItemProperties();
        PrevProps = chessItemProtertiesFactory.CreateNullChessItemProperties();
        mm = FindObjectOfType<MainMenu>();
        isMate = false;
    }

    protected void Update()
    {
        if (!isMatchPlaying)
            return;
        if (isMateChecked)
            return;


        if (doUpdateBoardPosition)
        {
            doUpdateBoardPosition = false;
            IsBoardPositionUpdated = true;
            SetBoardPosition();
        }

        if (do_action)
        {
            do_action = false;
            iChessAction.MakeAction(BoardPosition.None);
        }

        isMateChecked = isMate;
    }

    public void StartMatch()
    {
        isMatchPlaying = true;
    }

    public void FinishMove()
    {
        if (!isMatchPlaying)
            return;
        if (isMateChecked)
            return;


        if (iChessMatchCurrentState.Match_Stage != MatchStage.mate)
        {
            if (iChessMatchCurrentState.CurrentPlayer.Type == PlayerType.computer)
                do_action = true;
        }
    }


    public void SelectFigure(Vector2Int pos)
    {
        var board_item = GetBoardItemMono(pos);
        foreach (var item in boardItemsMono)
        {
            if (item != board_item)
                item.IsSelected = false;
        }

        board_item.IsSelected = iChessMatchCurrentState.CurrentPlayer.Type != PlayerType.computer;
    }

    public virtual void MoveFigure(Vector2Int prev_pos, Vector2Int new_pos, ChessSide side)
    {
        foreach (var item in boardItemsMono)
            item.StopAnimation();

        GetBoardItemMono(prev_pos).IsSelected = false;
        GetChessItemMono(prev_pos).MoveFigureToPosition(new Vector2Int[1] { new_pos });

        //Check for pawn becomes queen
        var ch_m = GetChessItemMono(new_pos);
        if (!ch_m.props.isNullObject && ch_m.props.type == (byte)ClassicChessItemType.pawn)
        {
            if (ch_m.props.side == ChessSide.white && ch_m.props.pos.y == boardSizeY - 1 ||
                (ch_m.props.side == ChessSide.black && ch_m.props.pos.y == 0))
            {
                ch_m.SetChessItem((byte)ClassicChessItemType.queen, ch_m.props.side);
            }
        }

        currentProps = chessItemProtertiesFactory.CreateNullChessItemProperties();
        PrevProps = chessItemProtertiesFactory.CreateNullChessItemProperties();

        mm.SetMoveTextValue(prev_pos, new_pos);
    }

    public void KillFigure(Vector2Int pos)
    {
        var chess_item_mono = GetChessItemMono(pos);
        if (chess_item_mono != null)
        {
            var go = chess_item_mono.gameObject;
            chessItemsMono.Remove(chess_item_mono);
            Destroy(go);
        }
    }

    protected bool firstStepInNewGameOrAfrerRestoreDone = false;
    public void SelectBoardItem(Vector2Int pos)
    {
        if (!isMatchPlaying)
            return;
        if (isMateChecked)
            return;

        if (!firstStepInNewGameOrAfrerRestoreDone)
        {
            if (iChessMatchCurrentState.CurrentPlayer.Type == PlayerType.computer)
            {
                do_action = true;
                return;
            }
        }

        ChessSide side;
        byte type;
        bool isChessOnBoard = TryGetChessSide(pos, out side, out type);

        //if selected board item is empty
        if (!isChessOnBoard)
        {
            //if chess item was not selected
            if (currentProps.IsNullObject())
                return;
        }
        //if selected board item is not empty
        else
        {
            //if chess item was not selected
            if (currentProps.IsNullObject())
            {
                if (side != iChessMatchCurrentState.CurrentPlayer.Side)
                    return;
            }

            foreach (var item in boardItemsMono)
                item.StopAnimation();

            if (side == iChessMatchCurrentState.CurrentPlayer.Side)
            {
                var boardPos = pos.ToBoardPosition();
                if (GetChessItemMono(pos) == null)
                    boardPos.IsNullObject = true;

                List<bool> temp;
                var possibleMoves = iFigureOnBoard.GetPossibleMoves(boardPos, out temp);
                foreach (var poss_move in possibleMoves)
                {
                    if (poss_move.ToVector2Int() != pos)
                        GetBoardItemMono(poss_move.ToVector2Int()).PossibleMoveAnimation();
                }

                possibleMoveList = possibleMoves;
                isJumpList = temp;
            }
        }

        PrevProps = currentProps;
        if (!isChessOnBoard)
            currentProps = new ChessItemProperties(pos, type, side, false);
        else
            currentProps = new ChessItemProperties(pos, type, side, false);

        if (!firstStepInNewGameOrAfrerRestoreDone ||
            (firstStepInNewGameOrAfrerRestoreDone && iChessMatchCurrentState.CurrentPlayer.Type == PlayerType.human))
        {
            var boardPos = currentProps.pos.ToBoardPosition();
            if (GetChessItemMono(currentProps.pos) == null || GetChessItemMono(currentProps.pos).props.isNullObject)
                boardPos.IsNullObject = true;
            iChessAction.MakeAction(boardPos);
        }

        firstStepInNewGameOrAfrerRestoreDone = true;
    }

    private bool TryGetChessSide(Vector2Int pos, out ChessSide side, out byte type)
    {
        foreach (var item in chessItemsMono)
        {
            if (item.props.pos == pos)
            {
                side = item.props.side;
                type = item.props.type;
                return true;
            }
        }
        side = ChessSide.black;
        type = 255;
        return false;
    }

    public void SetCheck(ChessSide side)
    {
        string message = "Check for " + side.ToString();
        Debug.Log(message);
        MainMenu.Singleton.AppendConsoleText(message);
    }

    public void SetMate(ChessSide side)
    {
        string message = "Mate for " + side.ToString();
        Debug.Log(message);
        MainMenu.Singleton.AppendConsoleText(message);
        isMate = true;
    }

    public void SetScreenPosition(int x_pos, int y_pos)
    {
        //if (!isMatchPlaying)
        //    return;
        if (isMateChecked)
            return;

        ScreenPosition = new Vector2Int(x_pos, y_pos);
    }

    protected virtual void SetBoardPosition()
    {
        var gameWindowSize = ASmirnov.UnityCustoms.GetGameWindowSize();
        Vector3 zero_point;
        float board_width, board_height;
        float b = (1 - delta_bottom_coeff - delta_top_coeff) * gameWindowSize.y;
        float c = (float)boardSizeX / (float)boardSizeY;

        if (c > gameWindowSize.x / b)
        {
            zero_point = Camera.main.ScreenToWorldPoint(new Vector3(0, delta_bottom_coeff * gameWindowSize.y + (b - gameWindowSize.x / c) / 2f, 2f));
            board_width = Camera.main.ScreenToWorldPoint(new Vector3(gameWindowSize.x, 0, 0)).x - zero_point.x;
            board_height = board_width / c;
        }
        else
        {
            zero_point = Camera.main.ScreenToWorldPoint(new Vector3((gameWindowSize.x - c * b) / 2f, delta_bottom_coeff * gameWindowSize.y, 2f));
            board_height = Camera.main.ScreenToWorldPoint(new Vector3(0, b + delta_bottom_coeff * gameWindowSize.y, 0)).y - zero_point.y;
            board_width = c * board_height;
        }

        boardSpriteSize.x = board_width / (float)boardSizeX;
        boardSpriteSize.y = board_height / (float)boardSizeY;

        BoardPositionMono = new Vector3(
            zero_point.x + board_width * (float)ScreenPosition.x / (float)boardSizeX + boardSpriteSize.x / 2f,
            zero_point.y + board_height * (float)ScreenPosition.y / (float)boardSizeY + boardSpriteSize.y / 2f,
            0);
    }

    protected Vector3 GetBoardPosition(int x, int y)
    {
        return GetBoardPosition(new Vector2Int(x, y));
    }

    private Vector3 GetBoardPosition(Vector2Int screen_pos)
    {
        SetScreenPosition(screen_pos.x, screen_pos.y);
        SetBoardPosition();
        doUpdateBoardPosition = false;
        return BoardPositionMono;
    }

    public ChessItemProperties GetChessItemProperties(Vector2Int pos)
    {
        return board[pos.x, pos.y];
    }

    public ChessItemMonoBase GetChessItemMono(Vector2Int pos)
    {
        foreach (var item in chessItemsMono)
        {
            if (item.props.pos == pos)
                return item;
        }
        return null;
    }

    public BoardItemMonoBase GetBoardItemMono(Vector2Int pos)
    {
        foreach (var item in boardItemsMono)
        {
            if (item.pos == pos)
                return item;
        }
        return null;
    }

    protected virtual void InitBoardItems()
    {
        
        UnityCustoms.DestroyAllChilds(board_items_parent = GameObject.Find(ChessConstants.BoardItemsObjectName));
        UnityCustoms.DestroyGameObjects(ChessConstants.BoardItemsObjectName);
        board_items_parent = new GameObject(ChessConstants.BoardItemsObjectName);
        board_items_parent.transform.SetParent(GameObject.Find(ChessConstants.BoardObjectName).transform);

        boardItemsMono = new List<BoardItemMonoBase>();

        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                GameObject item_obj = GameObject.Instantiate(
                    (i + j) % 2 == 0 ? MainMenu.Singleton.GetCurrentBoardPrefabs().dark_board_item : MainMenu.Singleton.GetCurrentBoardPrefabs().bright_board_item) as GameObject;
                item_obj.name = "board_item_x_" + i + "_y_" + j;

                BoardItemMono item_scr = item_obj.GetComponent<BoardItemMono>();
                item_scr.SetPosition(new Vector2Int(i, j));
                boardItemsMono.Add(item_scr);
                item_obj.transform.SetParent(board_items_parent.transform);
                item_obj.transform.position = GetBoardPosition(i, j);
                UnityCustoms.SetSpriteRendererObjectSize(item_obj, boardSpriteSize);
            }
        }
    }

    protected virtual void InitChessItems()
    {
        UnityCustoms.DestroyAllChilds(chess_items_parent = GameObject.Find(ChessConstants.ChessItemsObjectName));
        UnityCustoms.DestroyGameObjects(ChessConstants.ChessItemsObjectName);
        chess_items_parent = new GameObject(ChessConstants.ChessItemsObjectName);
        chess_items_parent.transform.SetParent(GameObject.Find(ChessConstants.BoardObjectName).transform);

        chessItemsMono = new List<ChessItemMonoBase>();
        int idx = 0;
        for (int i = 0; i < boardSizeX; i++)
        {
            for (int j = 0; j < boardSizeY; j++)
            {
                GameObject chess_obj = null;
                ChessItemProperties props = board[i, j];
                if (!props.isNullObject)
                {
                    if (new byte[]
                    {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].pawn) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (new byte[]
                    {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].rook) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (new byte[]
                    {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].knight) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (new byte[]
                    {
                        (byte)ClassicChessItemType.bishop,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].bishop) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (new byte[]
                    {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].queen) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (new byte[]
                    {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].king) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }


                    ChessItemMonoBase chess_scr = chess_obj.GetComponent<ChessItemMono>();
                    chess_scr.props = props;
                    chessItemsMono.Add(chess_scr);
                    chess_obj.transform.SetParent(chess_items_parent.transform);
                    Vector3 pos = GetBoardPosition(i, j);
                    chess_obj.transform.position = new Vector3(pos.x, pos.y, pos.z - 0.1f);
                    UnityCustoms.SetSpriteRendererObjectSize(chess_obj, boardSpriteSize);
                }
            }
        }
    }

    public void InitObjects(IChessMatchCurrentState iChessMatchCurrentState, IChessAction iChessAction, IMoveBack iMoveBack, IFigureOnBoard iFigureOnBoard)
    {
        //if (!isMatchPlaying)
        //    return;
        if (isMateChecked)
            return;


        this.iChessMatchCurrentState = iChessMatchCurrentState;
        this.iChessAction = iChessAction;
        this.iMoveBack = iMoveBack;
        this.iFigureOnBoard = iFigureOnBoard;

        if (GameObject.Find(ChessConstants.BoardObjectName) == null)
            new GameObject(ChessConstants.BoardObjectName);

        CreateBoard(iFigureOnBoard.BoardSizeX, iFigureOnBoard.BoardSizeY);
        GetBoardData();
        InitBoardItems();
        InitChessItems();
    }

    private void CreateBoard(byte size_x, byte size_y)
    {
        boardSizeX = size_x;
        boardSizeY = size_y;
        board = new ChessItemProperties[boardSizeX, boardSizeY];
    }

    public void GetBoardData()
    {
        for (byte i = 0; i < boardSizeX; i++)
            for (byte j = 0; j < boardSizeY; j++)
            {
                IChessItemModelShort c = iFigureOnBoard.GetFigureByPosition(new BoardPosition(i, j));
                if (!c.IsNullObject)
                    board[i, j] = new ChessItemProperties(c.Pos.ToVector2Int(), c.Type, c.Side);
                else
                    board[i, j] = new ChessItemProperties(new Vector2Int(225, 225), c.Type, c.Side, true);
            }
    }

    public void RecoveryChessItem(ChessItemProperties props)
    {
        GameObject chess_obj = null;
        if (!props.isNullObject)
        {
            if (new byte[]
            {
                        (byte)ClassicChessItemType.pawn,
                        (byte)LosAlamosChessItemType.pawn,
                        (byte)ChaturangaChessItemType.pawn,
                        (byte)CircledChessItemType.pawn_left,
                        (byte)CircledChessItemType.pawn_right
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].pawn) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }
            else if (new byte[]
            {
                        (byte)ClassicChessItemType.rook,
                        (byte)LosAlamosChessItemType.rook,
                        (byte)ChaturangaChessItemType.rook,
                        (byte)CircledChessItemType.rook
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].rook) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }
            else if (new byte[]
            {
                        (byte)ClassicChessItemType.knight,
                        (byte)LosAlamosChessItemType.knight,
                        (byte)ChaturangaChessItemType.knight,
                        (byte)CircledChessItemType.knight
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].knight) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }
            else if (new byte[]
            {
                        (byte)ClassicChessItemType.bishop,
                        (byte)CircledChessItemType.bishop
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].bishop) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }
            else if (new byte[]
            {
                        (byte)ClassicChessItemType.queen,
                        (byte)LosAlamosChessItemType.queen,
                        (byte)ChaturangaChessItemType.queen,
                        (byte)CircledChessItemType.queen
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].queen) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }
            else if (new byte[]
            {
                        (byte)ClassicChessItemType.king,
                        (byte)LosAlamosChessItemType.king,
                        (byte)ChaturangaChessItemType.king,
                        (byte)CircledChessItemType.king
            }.Contains(props.type))
            {
                chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].king) as GameObject;
                chess_obj.name = props.side.ToString() + "_chess_";
            }

            ChessItemMono chess_scr = chess_obj.GetComponent<ChessItemMono>();
            chess_scr.props = props;
            chessItemsMono.Add(chess_scr);
            chess_obj.transform.SetParent(board_items_parent.transform);
            Vector3 pos = GetBoardPosition(props.pos.x, props.pos.y);
            chess_obj.transform.position = new Vector3(pos.x, pos.y, pos.z - 0.1f);

            UnityCustoms.SetSpriteRendererObjectSize(chess_obj, boardSpriteSize);
            chess_scr.RecoveryFigure();
        }
    }
}

