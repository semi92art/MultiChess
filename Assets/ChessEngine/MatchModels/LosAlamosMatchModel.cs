using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public sealed class LosAlamosMatchModel : MatchModelBase
    {
        public LosAlamosMatchModel(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, List<IPlayer> playerList, IGameLoaderSaverService gameLoaderSaverService) :
               base(fen, fromPositions, toPositions, playerList, ChessGameType.los_alamos, gameLoaderSaverService)
        {
            var fen_array = fen.Split(' ');
            var factory = new ChessBoardKitFactory();
            this.BoardKit = factory.GetLosAlamosChessBoardKit(fen_array[0]);

            SetBoardByPositionMoves(fromPositions, toPositions);
        }
    }
}
