

namespace ChessEngine
{
    public sealed class ChessBoardKitFactory
    {
        public IChessBoardKit GetClassicChessBoardKit(string short_fen)
        {
            var figPlacementFactory = new FigurePlacementFactory();
            var figuresPlacement = figPlacementFactory.FiguresPlacement(short_fen, 8, 8, ChessGameType.classic);
            return new ClassicChessBoardKit(figuresPlacement);
        }

        public IChessBoardKit GetLosAlamosChessBoardKit(string short_fen)
        {
            var figPlacementFactory = new FigurePlacementFactory();
            var figurePlacement = figPlacementFactory.FiguresPlacement(short_fen, 6, 6, ChessGameType.los_alamos);
            return new LosAlamosChessBoardKit(figurePlacement);
        }

        public IChessBoardKit GetChaturangaChessBoardKit(string short_fen)
        {
            var figPlacementFactory = new FigurePlacementFactory();
            var figurePlacement = figPlacementFactory.FiguresPlacement(short_fen, 8, 8, ChessGameType.chaturanga);
            return new ChaturangaChessBoardKit(figurePlacement);
        }

        public IChessBoardKit GetCircledChessBoardKit(string short_fen)
        {
            var figPlacementFactory = new FigurePlacementFactory();
            var figurePlacement = figPlacementFactory.FiguresPlacement(short_fen, 16, 4, ChessGameType.circled);
            return new CircledBoardKit(figurePlacement);
        }
    }
}
