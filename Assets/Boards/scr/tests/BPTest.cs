using UnityEngine;
using ChessEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class BPTest : MonoBehaviour
{
    public Vector2Int boardPosition;

    public void DisplayBPString()
    {
        Debug.Log(UciConverter.BoardPositionToString(boardPosition.ToBoardPosition()));
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(BPTest))]
public class BPTestEditor : Editor
{
    BPTest o;

    void OnEnable()
    {
        o = target as BPTest;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Display BP String"))
            o.DisplayBPString();
    }
}
#endif