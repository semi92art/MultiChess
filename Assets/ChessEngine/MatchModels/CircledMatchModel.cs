﻿using System;
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class CircledMatchModel : MatchModelBase
    {
        public CircledMatchModel(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, List<IPlayer> playerList, IGameLoaderSaverService gameLoaderSaverService) :
            base(fen, fromPositions, toPositions, playerList, ChessGameType.circled, gameLoaderSaverService)
        {
            var fen_array = fen.Split(' ');
            var factory = new ChessBoardKitFactory();
            this.BoardKit = factory.GetCircledChessBoardKit(fen_array[0]);

            SetBoardByPositionMoves(fromPositions, toPositions);
        }
    }
}
