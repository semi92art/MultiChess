using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ChessEngine;

public class SavedGameMenuItem : MonoBehaviour
{
    public Text FileName_text;
    public string FilePath { get; private set; }

    private void Start()
    {
        transform.localScale = Vector3.one;
    }

    public void InitItem(string filePath)
    {
        FilePath = filePath;
        FileName_text.text = filePath.Substring(filePath.LastIndexOf('/') + 1, filePath.Length - filePath.LastIndexOf('/') - 1);
    }

    public void LoadGame()
    {
        ChessGameType gameType;
        List<BoardPosition> fromPositions;
        List<BoardPosition> toPositions;
        ServiceLocator.Singleton.GameLoaderSaverService.LoadGame(FilePath, out gameType, out fromPositions, out toPositions);

        var controllerFactory = new ControllerFactory();
        switch (gameType)
        {
            case ChessGameType.classic:
                MainMenu.Singleton.SetClassicMatchMenu();
                MainMenu.Singleton.StartClassicMatch(ChessEngineConstants.FEN_Classic_Default, fromPositions, toPositions);
                break;
            case ChessGameType.los_alamos:
                MainMenu.Singleton.SetLosAlamosMatchMenu();
                MainMenu.Singleton.StartLosAlamosMatch(ChessEngineConstants.FEN_LosAlamos_Default, fromPositions, toPositions);
                break;
            case ChessGameType.chaturanga:
                MainMenu.Singleton.SetChaturangaMatchMenu();
                MainMenu.Singleton.StartChaturangaMatch(ChessEngineConstants.FEN_Chaturanga_Default, fromPositions, toPositions);
                break;
            case ChessGameType.circled:
                MainMenu.Singleton.SetCircledMatchMenu();
                MainMenu.Singleton.StartCircledMatch(ChessEngineConstants.FEN_Circled_Default, fromPositions, toPositions);
                break;
            default:
                break;
        }
    }
}
