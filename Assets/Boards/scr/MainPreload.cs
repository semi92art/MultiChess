using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using ChessEngine;





public class MainPreload : MonoBehaviour
{
    private static MainPreload Singleton;


    public BoardPrefabsPlayerProperties classic_board_and_players;
    public BoardPrefabsPlayerProperties los_alamos_board_and_players;
    public BoardPrefabsPlayerProperties chaturanga_board_and_players;
    public BoardPrefabsPlayerProperties circled_board_and_players;
    public GameObject saved_game_menu_item_object;
    public Material board_render_material_white;
    public Material board_render_material_black;
    public Material board_render_material_select;
    public Material board_redner_material_possible_move;

    public static BoardPrefabsPlayerProperties Classic_board_and_players
    {
        get { return Singleton.classic_board_and_players; }
    }
    public static BoardPrefabsPlayerProperties Los_alamos_board_and_players
    {
        get { return Singleton.los_alamos_board_and_players; }
    }
    public static BoardPrefabsPlayerProperties Chaturanga_board_and_players
    {
        get { return Singleton.chaturanga_board_and_players; }
    }
    public static BoardPrefabsPlayerProperties Circled_board_and_players
    {
        get { return Singleton.circled_board_and_players; }
    }
    public static GameObject Saved_game_menu_item_object
    {
        get { return Singleton.saved_game_menu_item_object; }
    }
    public static Material Board_render_material_white
    {
        get { return Singleton.board_render_material_white; }
    }
    public static Material Board_render_material_black
    {
        get { return Singleton.board_render_material_black; }
    }

    public static Material Board_render_material_select
    {
        get { return Singleton.board_render_material_select; }
    }

    public static Material Board_redner_material_possible_move
    {
        get { return Singleton.board_redner_material_possible_move; }
    }




    private void Start()
    {
        Singleton = this;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            DestroyAllExceptThis();
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene(1);
        }
    }

    private void DestroyAllExceptThis()
    {
        foreach (var item in FindObjectsOfType<GameObject>())
        {
            if (item != gameObject)
            {
#if UNITY_EDITOR
                DestroyImmediate(item);
#else
                Destroy(item);
#endif
            }
        }
    }
}



