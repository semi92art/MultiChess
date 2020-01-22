
using System.Collections.Generic;

namespace ChessEngine
{
    public interface IGameLoaderSaverService
    {
        bool SaveGame(string file_name, List<FigureMove> moves, ChessGameType gameType);
        bool LoadGame(string filePath, out ChessGameType gameType, out List<BoardPosition> fromPositions, out List<BoardPosition> toPositions);
    }
}
