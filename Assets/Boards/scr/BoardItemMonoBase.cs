using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChessEngine;


public abstract class BoardItemMonoBase : MonoBehaviour
{
    public Vector2Int pos { get; private set; }

    protected IMatchView_Internal matchView;

    protected bool do_animate_select;
    protected bool do_stop_animate;
    protected bool do_animate_possible_move;

    protected bool isSelected;
    protected bool isOnPointer;
    protected bool wasOnPointer;

    public bool IsSelected
    {
        get { return isSelected; }
        set
        {
            SelectItem(value);
            isSelected = value;
        }
    }

    protected virtual void Start() { }

    protected virtual void Update()
    {
        OnPointerEnter();
    }

    public virtual void SetPosition(Vector2Int pos)
    {
        this.pos = pos;
    }

    protected virtual void SelectItem(bool select)
    {
        if (!select)
        {
            //StopAnimation();
            return;
        }

        SelectAnimation();
    }

    protected void OnPointerEnter()
    {
        

        wasOnPointer = isOnPointer;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            isOnPointer = transform == hit.transform;
            if (isOnPointer)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                if (isOnPointer && Input.GetMouseButtonDown(0))
                    SelectBoardItem();



#elif UNITY_ANDROID
                isOnPointer = Input.touchCount > 0;
                if (isOnPointer && !wasOnPointer)
                    SelectBoardItem();
#endif
            }
        }
        else
            isOnPointer = false;
    }

    public void SelectAnimation()
    {
        //if (pos.x == 5 && pos.y == 0)
        //    Debug.Log("SelectAnimation");
        do_animate_select = true;
    }

    public void PossibleMoveAnimation()
    {
        do_animate_possible_move = true;
    }

    public void StopAnimation()
    {
        //if (pos.x == 5 && pos.y == 0)
        //    Debug.Log("AnimatePossibleMove");
        do_stop_animate = true;
    }

    public void SelectBoardItem()
    {
        if (IsSelected)
            return;
        matchView.SelectBoardItem(pos);
    }

    public void SetGameobjectPositionAndScale(Vector3 position, Vector3 scale)
    {
        transform.position = position;
        transform.localScale = scale;
    }
}

