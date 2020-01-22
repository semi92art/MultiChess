using System.Collections.Generic;

namespace ChessEngine
{
    public interface IGameSaver
    {
        bool SaveGame(string file_name);
    }
}
