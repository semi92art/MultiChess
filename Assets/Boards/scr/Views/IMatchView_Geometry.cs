
using UnityEngine;

public interface IMatchView_Geometry
{
    Vector3 GetBoardCornerPosition(Vector2Int pos);
    bool IsJump(Vector2Int pos);
}
