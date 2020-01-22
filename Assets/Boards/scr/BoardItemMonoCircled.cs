using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessEngine;


public sealed class BoardItemMonoCircled : BoardItemMonoBase
{
    private Material renderMaterial;
    private IMatchView_Geometry matchView_Geometry;
    private bool started;
    private LineRenderer lineRenderer;
    private MeshCollider meshCollider;

    protected override void Start()
    {
        CircledMatchView mv = FindObjectOfType<CircledMatchView>();
        matchView = mv as IMatchView_Internal;
        matchView_Geometry = mv as IMatchView_Geometry;
        if (matchView == null)
            throw new System.NullReferenceException("Match View Not Found!");
    }

    protected override void Update()
    {
        base.Update();

        if (do_stop_animate)
        {
            do_stop_animate = false;
            StopAnimate();
        }

        if (do_animate_select)
        {
            do_animate_select = false;
            AnimateSelect();
        }

        if (do_animate_possible_move)
        {
            do_animate_possible_move = false;
            AnimatePossibleMove();
        }
    }

    private void AnimateSelect()
    {
        lineRenderer.enabled = true;
        lineRenderer.sharedMaterial = MainPreload.Board_render_material_select;
    }

    private void AnimatePossibleMove()
    {
        lineRenderer.enabled = true;
        lineRenderer.sharedMaterial = MainPreload.Board_redner_material_possible_move;
    }

    private void StopAnimate()
    {
        lineRenderer.enabled = false;
    }

    public override void SetPosition(Vector2Int pos)
    {
        if (!started)
            Start();
        

        base.SetPosition(pos);
        if ((pos.x + pos.y) % 2 == 0)
            renderMaterial = MainPreload.Board_render_material_black;
        else
            renderMaterial = MainPreload.Board_render_material_white;

        var corner00 = matchView_Geometry.GetBoardCornerPosition(pos);
        var corner01 = matchView_Geometry.GetBoardCornerPosition(new Vector2Int(pos.x + 1, pos.y));
        var corner11 = matchView_Geometry.GetBoardCornerPosition(new Vector2Int(pos.x + 1, pos.y + 1));
        var corner10 = matchView_Geometry.GetBoardCornerPosition(new Vector2Int(pos.x, pos.y + 1));
        var corners = new Vector3[4] { corner00, corner01, corner11, corner10 };

        MeshGenerator.GenerateBoardItemMesh(corners, gameObject, renderMaterial);

        if (!started)
        {
            meshCollider = gameObject.AddComponent<MeshCollider>();
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = 5;
            lineRenderer.SetPositions(corners);
            lineRenderer.SetPosition(4, corners[0]);
            lineRenderer.enabled = false;
            lineRenderer.numCornerVertices = 50;
            lineRenderer.widthMultiplier = 0.06f;
            lineRenderer.sharedMaterial = MainPreload.Board_render_material_select;
            lineRenderer.loop = true;
        }
        


        started = true;
    }

}

