using UnityEngine;
using ChessEngine;
using System.Collections;
using System.Collections.Generic;
using ASmirnov;
using System.Linq;

public sealed class ChessItemMonoCircled : ChessItemMonoBase
{
    protected override void Start()
    {
        base.Start();

        matchView = FindObjectOfType<CircledMatchView>() as IMatchView_Internal;

        if (matchView == null)
            throw new System.NullReferenceException(MainMenu.Singleton.GameType.ToString() + " Match View Not Found!");
    }

    protected override void Update()
    {
        if (doMove)
        {
            doMove = false;
            StartCoroutine(MoveFigure_(figure_path));
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

    protected override IEnumerator MoveFigure_(Vector2Int[] path)
    {
        sprRend.sortingOrder++;
        int k = -1;
        foreach (var screen_pos in path)
        {
            k++;
            matchView.SetScreenPosition(screen_pos.x, screen_pos.y);
            while (!matchView.IsBoardPositionUpdated)
                yield return null;
            Debug.Log("MoveFigure_ active phase!");
            var pos = matchView.BoardPositionMono;
            Vector3 prev_pos = transform.position;
            isMoving = true;

            System.DateTime dt_start = System.DateTime.Now;
            float start_dt = Time.time;

            var startPos = transform.position;
            while (isMoving && dt_start.SecondsBetween(System.DateTime.Now) < 5f)
            {
                pos.z = transform.position.z;
                transform.position += (pos - startPos) * Time.deltaTime * 4f;
                if (Vector3.Distance(pos, transform.position) < 0.05f)
                    isMoving = false;

                yield return null;
            }
        }



        sprRend.sortingOrder--;
        matchView.FinishMove();
    }
}

