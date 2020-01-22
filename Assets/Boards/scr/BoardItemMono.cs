using UnityEngine;

public sealed class BoardItemMono : BoardItemMonoBase
{
    private Animator anim;

    protected override void Start()
    {
        anim = GetComponent<Animator>();

        switch (MainMenu.Singleton.GameType)
        {
            case ChessEngine.ChessGameType.classic:
                matchView = FindObjectOfType<ClassicChessMatchView>() as IMatchView_Internal;
                break;
            case ChessEngine.ChessGameType.los_alamos:
                matchView = FindObjectOfType<LosAlamosMatchView>() as IMatchView_Internal;
                break;
            case ChessEngine.ChessGameType.chaturanga:
                matchView = FindObjectOfType<ChaturangaMatchView>() as IMatchView_Internal;
                break;
            default:
                throw new System.NotImplementedException("Start Function Was Not Implemented Completely!");
        }

        if (matchView == null)
            throw new System.NullReferenceException("Match View Not Found!");
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (do_animate_select)
        {
            do_animate_select = false;
            anim.SetTrigger("select");
        }

        if (do_animate_possible_move)
        {
            do_animate_possible_move = false;
            anim.SetTrigger("poss_move");
        }

        if (do_stop_animate)
        {
            do_stop_animate = false;
            anim.SetTrigger("stop");
        }
    }
}

