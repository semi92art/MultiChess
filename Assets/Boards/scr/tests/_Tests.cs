using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASmirnov;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class _Tests : MonoBehaviour
{
    public Camera cam;
    [Range(0, 1)]
    public float x_screen_coord;
    [Range(0, 1)]
    public float y_screen_coord;

    [Range(0.1f, 10f)]
    public float proportion = 1f;

    private Vector3 point;
    private Vector3[] squarePoints;

    private void Update()
    {
        DrawScaledCubeInScreenCenter();
    }

    public void SetPointByScreenNormalizedCoords()
    {
        Vector2 size = UnityCustoms.GetGameWindowSize();
        float x_pos = x_screen_coord * size.x;
        float y_pos = y_screen_coord * size.y;
        point = cam.ScreenToWorldPoint(new Vector3(x_pos, y_pos, 0));
        point.z += 2;
    }

    public void DrawScaledCubeInScreenCenter()
    {
        if (squarePoints == null || squarePoints.Length == 0)
            squarePoints = new Vector3[4];

        float c_to_center = 0f;
        var size = UnityCustoms.GetGameWindowSize();
        if (size.x > size.y)
        {
            if (proportion < size.x / size.y)
            {
                squarePoints[0] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f - proportion * size.y / 2f, 0 + c_to_center, 2f));
                squarePoints[1] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f + proportion * size.y / 2f, 0 + c_to_center, 2f));
                squarePoints[2] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f + proportion * size.y / 2f, size.y - c_to_center, 2f));
                squarePoints[3] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f - proportion * size.y / 2f, size.y - c_to_center, 2f));
            }
            else
            {
                squarePoints[0] = cam.ScreenToWorldPoint(new Vector3(0 + c_to_center, size.y / 2f - size.x / (2f * proportion), 2f));
                squarePoints[1] = cam.ScreenToWorldPoint(new Vector3(0 + c_to_center, size.y / 2f + size.x / (2f * proportion), 2f));
                squarePoints[2] = cam.ScreenToWorldPoint(new Vector3(size.x - c_to_center, size.y / 2f + size.x / (2f * proportion), 2f));
                squarePoints[3] = cam.ScreenToWorldPoint(new Vector3(size.x - c_to_center, size.y / 2f - size.x / (2f * proportion), 2f));
            }
        }
        else
        {
            if (proportion > size.x / size.y)
            {
                squarePoints[0] = cam.ScreenToWorldPoint(new Vector3(0 + c_to_center, size.y / 2f - size.x / (2f * proportion), 2f));
                squarePoints[1] = cam.ScreenToWorldPoint(new Vector3(0 + c_to_center, size.y / 2f + size.x / (2f * proportion), 2f));
                squarePoints[2] = cam.ScreenToWorldPoint(new Vector3(size.x - c_to_center, size.y / 2f + size.x / (2f * proportion), 2f));
                squarePoints[3] = cam.ScreenToWorldPoint(new Vector3(size.x - c_to_center, size.y / 2f - size.x / (2f * proportion), 2f));
            }
            else
            {
                squarePoints[0] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f - proportion * size.y / 2f, 0 + c_to_center, 2f));
                squarePoints[1] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f + proportion * size.y / 2f, 0 + c_to_center, 2f));
                squarePoints[2] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f + proportion * size.y / 2f, size.y - c_to_center, 2f));
                squarePoints[3] = cam.ScreenToWorldPoint(new Vector3(size.x / 2f - proportion * size.y / 2f, size.y - c_to_center, 2f));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point, 0.1f);

        if (squarePoints != null && squarePoints.Length > 0)
        {
            Gizmos.DrawLine(squarePoints[0], squarePoints[1]);
            Gizmos.DrawLine(squarePoints[1], squarePoints[2]);
            Gizmos.DrawLine(squarePoints[2], squarePoints[3]);
            Gizmos.DrawLine(squarePoints[3], squarePoints[0]);
            Gizmos.DrawLine(squarePoints[0], squarePoints[2]);
            Gizmos.DrawLine(squarePoints[1], squarePoints[3]);
        }
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(_Tests))]
public class _TestsEditor : Editor
{
    private _Tests o;

    private void OnEnable()
    {
        o = target as _Tests;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Set Point By Screen NormCoords"))
            o.SetPointByScreenNormalizedCoords();
    }
}
#endif


