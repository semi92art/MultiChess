

namespace ChessEngine
{
    public sealed class ServiceLocator
    {
        public static ServiceLocator Singleton;
        public IChessUCIEngine ChessUCIEngine { get; private set; }
        public IGameLoaderSaverService GameLoaderSaverService { get; private set; }

        public ServiceLocator(IChessUCIEngine chessUCIEngine, IGameLoaderSaverService gameLoaderSaverService)
        {
            Singleton = this;
            ChessUCIEngine = chessUCIEngine;
            GameLoaderSaverService = gameLoaderSaverService;
        }
    }
}
