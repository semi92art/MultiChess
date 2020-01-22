using UnityEngine;
using System.Runtime.InteropServices;


#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class _Tests2 : MonoBehaviour
{
    public int a, b;

    public void PrintSum()
    {
        Debug.Log(Sum(a, b));
    }


    [DllImport("chess_engine", EntryPoint ="Sum", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern System.IntPtr Sum(int a, int b);
    [DllImport("chess_engine", EntryPoint = "InitEngine", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitEngine();
    [DllImport("chess_engine", EntryPoint ="GetCommand", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
    public static extern void GetCommand([MarshalAs(UnmanagedType.LPStr)] string cmd);
}

#if UNITY_EDITOR
[CustomEditor(typeof(_Tests2))]
public class _Tests2Editor : Editor
{
    private _Tests2 o;

    private void OnEnable()
    {
        o = target as _Tests2;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Call extern function"))
        {
            o.PrintSum();
        }
    }
}
#endif

