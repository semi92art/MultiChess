using UnityEngine;
using ChessEngine;
using System.Linq;
using System.Collections.Generic;
using ASmirnov;


public sealed class CircledMatchView : MatchViewBase, IMatchView_Geometry
{
    protected override void InitChessItems()
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
                        (byte)CircledChessItemType.pawn_left,
                        (byte)CircledChessItemType.pawn_right
                    }.Contains(props.type))
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].pawn) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (props.type == (byte)CircledChessItemType.rook)
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].rook) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (props.type == (byte)CircledChessItemType.knight)
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].knight) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (props.type == (byte)CircledChessItemType.bishop)
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].bishop) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (props.type == (byte)CircledChessItemType.queen)
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].queen) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }
                    else if (props.type == (byte)CircledChessItemType.king)
                    {
                        chess_obj = GameObject.Instantiate(MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)props.side].king) as GameObject;
                        chess_obj.name = props.side.ToString() + "_chess_" + idx++;
                    }


                    ChessItemMonoBase chess_scr = chess_obj.GetComponent<ChessItemMonoCircled>();
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

    protected override void SetBoardPosition()
    {
        BoardPositionMono = GetBoardMiddlePosition(ScreenPosition);
    }

    protected override void InitBoardItems()
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
                GameObject item_obj = new GameObject("board_item_x_" + i + "_y_" + j);
                var item_scr = item_obj.AddComponent<BoardItemMonoCircled>();
                item_scr.SetPosition(new Vector2Int(i, j));
                boardItemsMono.Add(item_scr);
                item_obj.transform.SetParent(board_items_parent.transform);
                //item_obj.transform.position = GetBoardPosition(i, j);
                UnityCustoms.SetSpriteRendererObjectSize(item_obj, boardSpriteSize);
            }
        }
    }

    private Vector3 mid_pos = Vector3.zero;
    private Vector3 GetBoardMiddlePosition(Vector2Int pos)
    {
        var corners = new Vector3[4];
        corners[0] = GetBoardCornerPosition(new Vector2Int(pos.x + 1, pos.y));
        corners[1] = GetBoardCornerPosition(new Vector2Int(pos.x, pos.y + 1));
        corners[2] = GetBoardCornerPosition(new Vector2Int(pos.x + 1, pos.y + 1));
        corners[3] = GetBoardCornerPosition(new Vector2Int(pos.x, pos.y));

        var result = Vector3.zero;
        foreach (var corner in corners)
            result += corner;
        result /= (float)corners.Length;

        mid_pos = result;
        return result;
        //return Vector3.zero;
    }
    
    public Vector3 GetBoardCornerPosition(Vector2Int pos)
    {
        var gameWindowSize = ASmirnov.UnityCustoms.GetGameWindowSize();
        Vector3 zero_point;
        float board_width, board_height;
        float b = (1 - delta_bottom_coeff - delta_top_coeff) * gameWindowSize.y;
        float c = (float)boardSizeX / (float)boardSizeY;

        float boardRadius;


        if (c > gameWindowSize.x / b)
        {
            zero_point = Camera.main.ScreenToWorldPoint(new Vector3(0, delta_bottom_coeff * gameWindowSize.y + (b - gameWindowSize.x / c) / 2f, 2f));
            board_width = Camera.main.ScreenToWorldPoint(new Vector3(gameWindowSize.x, 0, 0)).x - zero_point.x;
            board_height = board_width / c;

            boardRadius = board_width / 2;
        }
        else
        {
            zero_point = Camera.main.ScreenToWorldPoint(new Vector3((gameWindowSize.x - c * b) / 2f, delta_bottom_coeff * gameWindowSize.y, 2f));
            board_height = Camera.main.ScreenToWorldPoint(new Vector3(0, b + delta_bottom_coeff * gameWindowSize.y, 0)).y - zero_point.y;
            board_width = c * board_height;

            boardRadius = board_height / 2;
        }

        float coeff_otstup = 2f;
        float len_plosh = boardRadius / (4f + coeff_otstup);
        float radius_otstup = coeff_otstup * len_plosh;

        Vector3 corner;
        float Xo, Yo, Zo;

        Xo = zero_point.x + board_width / 2f;
        Yo = zero_point.y + board_height / 2f;
        Zo = 0;

        corner.x = Xo + (radius_otstup + (float)pos.y * len_plosh) * Mathf.Cos((float)pos.x * Mathf.PI / 8f);
        corner.y = Yo + (radius_otstup + (float)pos.y * len_plosh) * Mathf.Sin((float)pos.x * Mathf.PI / 8f);
        corner.z = Zo;

        boardSpriteSize.x = board_width / (float)boardSizeX;
        boardSpriteSize.y = board_height / (float)boardSizeY;

        return corner;
    }

    public override void MoveFigure(Vector2Int prev_pos, Vector2Int new_pos, ChessSide side)
    {
        foreach (var item in boardItemsMono)
            item.StopAnimation();

        GetBoardItemMono(prev_pos).IsSelected = false;

        var figure_path = PathFinder.GetPath(
            GetChessItemMono(prev_pos).props.type,
            prev_pos.ToBoardPosition(),
            new_pos.ToBoardPosition(),
            new BoardPosition(iFigureOnBoard.BoardSizeX, iFigureOnBoard.BoardSizeY),
            IsJump(new_pos));

        GetChessItemMono(prev_pos).MoveFigureToPosition( figure_path.ToVector2List() );



        currentProps = chessItemProtertiesFactory.CreateNullChessItemProperties();
        PrevProps = chessItemProtertiesFactory.CreateNullChessItemProperties();

        mm.SetMoveTextValue(prev_pos, new_pos);
    }

    public bool IsJump(Vector2Int pos)
    {
        int k = -1;
        foreach (var item in possibleMoveList)
        {
            k++;
            if (item.ToVector2Int() == pos)
                return isJumpList[k];
        }
        throw new System.NotImplementedException("IsJump Not Implemented Completely!");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(mid_pos, 0.03f);
    }
}

