using UnityEngine;
using ChessEngine;
using System.Collections;
using System.Collections.Generic;
using ASmirnov;
using System.Linq;

public abstract class ChessItemMonoBase : MonoBehaviour
{
    public SpriteRenderer sprRend;
    public ChessItemProperties props;

    protected Animator anim;
    protected IMatchView_Internal matchView;

    protected bool doMove;
    protected bool doKill;
    protected bool doRecovery;

    protected bool isMoving;
    protected Vector2Int screen_pos;
    protected Vector2Int[] figure_path;

    protected virtual void Start()
    {
        sprRend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (doRecovery)
            sprRend.color = new Color(1, 1, 1, 0);
    }

    protected virtual void Update()
    {
        if (doMove)
        {
            doMove = false;
            StartCoroutine(MoveFigure_(new Vector2Int[] { props.pos }));
        }

        if (doKill)
        {
            doKill = false;
            StartCoroutine(KillFigure_());
        }

        if (doRecovery)
        {
            doRecovery = false;
            StartCoroutine(RecoveryFigure_());
        }
    }

    protected virtual IEnumerator MoveFigure_(Vector2Int[] path)
    {
        sprRend.sortingOrder++;

        var screen_pos = path[0];
        matchView.SetScreenPosition(screen_pos.x, screen_pos.y);
        while (!matchView.IsBoardPositionUpdated)
            yield return null;
        var pos = matchView.BoardPositionMono;
        Vector3 prev_pos = transform.position;
        isMoving = true;

        System.DateTime dt_start = System.DateTime.Now;
        float start_dt = Time.time;
        while (Time.time - start_dt < 0.2f)
            yield return null;

        while (isMoving && dt_start.SecondsBetween(System.DateTime.Now) < 2f)
        {
            if (Vector3.Distance(pos, transform.position) > Vector3.Distance(prev_pos, pos) * 0.5f)
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 3f * Vector3.Distance(prev_pos, pos));
            else if (Vector3.Distance(pos, transform.position) > Vector3.Distance(prev_pos, pos) * 0.01f)
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 5f);
            else
                isMoving = false;
            yield return null;
        }
        sprRend.sortingOrder--;

        matchView.FinishMove();
    }

    protected virtual IEnumerator KillFigure_()
    {
        bool isFigureAlive = true;
        while (isFigureAlive)
        {
            sprRend.color = new Color(
                sprRend.color.r,
                sprRend.color.g,
                sprRend.color.b,
                sprRend.color.a - 0.05f);
            if (sprRend.color.a < 0.06f)
            {
                sprRend.color = new Color(0, 0, 0, 0);
                isFigureAlive = false;
            }
            yield return null;
        }
    }

    protected virtual IEnumerator RecoveryFigure_()
    {

        bool isFigureDead = true;
        while (isFigureDead)
        {
            sprRend.color = new Color(
                sprRend.color.r,
                sprRend.color.g,
                sprRend.color.b,
                sprRend.color.a + 0.02f);
            if (sprRend.color.a > 0.99f)
            {
                sprRend.color = new Color(1, 1, 1, 1);
                isFigureDead = false;
            }
            yield return null;
        }
    }




    public void MoveFigureToPosition(Vector2Int[] figure_path)
    {
        props.pos = figure_path.Last();
        this.figure_path = figure_path;
        doMove = true;
    }

    public void KillFigure()
    {
        doKill = true;
    }

    public void RecoveryFigure()
    {
        doRecovery = true;
    }

    public void SetChessItem(byte type, ChessSide side)
    {
        props.side = side;
        props.type = type;

        if (new byte[] {
            (byte)ClassicChessItemType.pawn,
            (byte)LosAlamosChessItemType.pawn,
            (byte)ChaturangaChessItemType.pawn,
            (byte)CircledChessItemType.pawn_left,
            (byte)CircledChessItemType.pawn_right
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].pawn.GetComponent<SpriteRenderer>().sprite;
        else if (new byte[] {
            (byte)ClassicChessItemType.rook,
            (byte)LosAlamosChessItemType.rook,
            (byte)ChaturangaChessItemType.rook,
            (byte)CircledChessItemType.rook
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].rook.GetComponent<SpriteRenderer>().sprite;
        else if (new byte[] {
            (byte)ClassicChessItemType.knight,
            (byte)LosAlamosChessItemType.knight,
            (byte)ChaturangaChessItemType.knight,
            (byte)CircledChessItemType.knight
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].knight.GetComponent<SpriteRenderer>().sprite;
        else if (new byte[] {
            (byte)ClassicChessItemType.bishop,
            (byte)CircledChessItemType.bishop
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].bishop.GetComponent<SpriteRenderer>().sprite;
        else if (new byte[] {
            (byte)ClassicChessItemType.queen,
            (byte)LosAlamosChessItemType.queen,
            (byte)ChaturangaChessItemType.queen,
            (byte)CircledChessItemType.queen
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].queen.GetComponent<SpriteRenderer>().sprite;
        else if (new byte[] {
            (byte)ClassicChessItemType.king,
            (byte)LosAlamosChessItemType.king,
            (byte)ChaturangaChessItemType.king,
            (byte)CircledChessItemType.king
        }.Contains(type))
            sprRend.sprite = MainMenu.Singleton.GetCurrentBoardPrefabs().chessPackList[(byte)side].king.GetComponent<SpriteRenderer>().sprite;
    }
}

