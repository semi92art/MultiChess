using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public sealed class ChaturangaMatchModel : MatchModelBase
    {
        public ChaturangaMatchModel(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, List<IPlayer> playerList, IGameLoaderSaverService gameLoaderSaverService) :
               base(fen, fromPositions, toPositions, playerList, ChessGameType.chaturanga, gameLoaderSaverService)
        {
            var fen_array = fen.Split(' ');
            var factory = new ChessBoardKitFactory();
            this.BoardKit = factory.GetChaturangaChessBoardKit(fen_array[0]);

            SetBoardByPositionMoves(fromPositions, toPositions);
        }
    }
}
