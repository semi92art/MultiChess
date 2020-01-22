using System;


namespace ChessEngine
{
    public enum GameLoaderSaverType
    {
        FEN
    }

    public enum ChessEngineType
    {
        Stockfish
    }

    public class ServiceLocatorFactory
    {
        public void CreateServiceLocator(GameLoaderSaverType gameLoaderSaverType, ChessEngineType chessEngineType, string saveFilesPath)
        {
            IChessUCIEngine uci_engine;
            IGameLoaderSaverService gameLoaderSaverService;
            switch (gameLoaderSaverType)
            {
                case GameLoaderSaverType.FEN:
                    gameLoaderSaverService = new FENSaverLoader(saveFilesPath);
                    break;
                default:
                    throw new NotImplementedException("CreateServiceLocator Not Implemented Completely!");
            }

            switch (chessEngineType)
            {
                case ChessEngineType.Stockfish:
#if UNITY_EDITOR || UNITY_STANDALONE
                    uci_engine = new StockFishDllService();
#elif UNITY_ANDROID
                    uci_engine = new StockfishJNIService();
#else
                    uci_engine = new StockFishDllService();
#endif
                    break;
                default:
                    throw new NotImplementedException("CreateServiceLocator Not Implemented Completely!");
            }

            new ServiceLocator(uci_engine, gameLoaderSaverService);
        }
    }
}
