using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CustomMenu : MonoBehaviour
{
    [MenuItem("SceneLoader/Preload")]
    public static void LoadScene_preload()
    {
        EditorSceneManager.OpenScene("Assets/scen/preload.unity");
    }

    [MenuItem("SceneLoader/Main")]
    public static void LoadScene_main()
    {
        EditorSceneManager.OpenScene("Assets/scen/main.unity");
    }

    [MenuItem("SceneLoader/Test")]
    public static void LoadScene_Test()
    {
        EditorSceneManager.OpenScene("Assets/scen/test.unity");
    }
}
