using System;
using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ClassicMatchModel : MatchModelBase, ICastling
    {
        public Castling WhitePossibleCastling { get; private set; }
        public Castling BlackPossibleCastling { get; private set; }


        public ClassicMatchModel(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, List<IPlayer> playerList, IGameLoaderSaverService gameLoaderSaverService) :
            base(fen, fromPositions, toPositions, playerList, ChessGameType.classic, gameLoaderSaverService)
        {
            var fen_array = fen.Split(' ');
            var factory = new ChessBoardKitFactory();
            this.BoardKit = factory.GetClassicChessBoardKit(fen_array[0]);

            SetBoardByPositionMoves(fromPositions, toPositions);
        }

        protected override void TryMove(bool backInTime = false, bool send_events = true)
        {
            BoardPosition captureOnTheIsle;
            IChessItemModel killed_chess;

            var prev_item = BoardKit.GetFigureFromBoard(PreviousSelectedPosition);
            byte prev_item_type = prev_item.Type;
            bool prev_item_is_null = prev_item.IsNullObject;
            var curr_item = BoardKit.GetFigureFromBoard(CurrentSelectedPosition);
            byte curr_item_type = curr_item.Type;
            bool curr_item_is_null = curr_item.IsNullObject;

            bool tryMoveResult = BoardKit.TryMoveChessItem(CurrentPlayer.Side, PreviousSelectedPosition, CurrentSelectedPosition, out captureOnTheIsle, out killed_chess);
            if (tryMoveResult)
            {
                AllMoves.Add(new FigureMove(PreviousSelectedPosition, CurrentSelectedPosition, killed_chess, CurrentPlayer.Side));

                PreviousPlayer = Players[currentPlayerIndex];
                if (++currentPlayerIndex >= Players.Count)
                    currentPlayerIndex = 0;
                CurrentPlayer = Players[currentPlayerIndex];
            }

            //If move is ready
            if (tryMoveResult)
            {
                
                if (!prev_item_is_null && !curr_item_is_null)
                {
                    if (prev_item_type == (byte)ClassicChessItemType.king && curr_item_type == (byte)ClassicChessItemType.rook ||
                        prev_item_type == (byte)ClassicChessItemType.rook && curr_item_type == (byte)ClassicChessItemType.king)
                        TryMakeCastling(backInTime);
                    else
                    {
                        LastPlayerAction = PlayerAction.MoveAndKill;
                        if (send_events)
                            OnChanged_Invoke(PreviousPlayer.Side);
                    }
                }
                else
                {
                    LastPlayerAction = PlayerAction.Move;
                    if (send_events)
                        OnChanged_Invoke(PreviousPlayer.Side);
                }

                CurrentSelectedPosition = BoardPosition.None;
                PreviousSelectedPosition = BoardPosition.None;
                CheckForCheckAndMate(send_events);
            }
            else
            {
                CurrentSelectedPosition = PreviousSelectedPosition;
                PreviousSelectedPosition = BoardPosition.None;
            }
        }

        private void TryMakeCastling(bool backInTime = false, bool send_events = true)
        {
            var curr_item = BoardKit.GetFigureFromBoard(CurrentSelectedPosition);
            var prev_item = BoardKit.GetFigureFromBoard(PreviousSelectedPosition);

            if (curr_item.IsNullObject || prev_item.IsNullObject)
                return;
            if (curr_item.Side != prev_item.Side)
                return;

            int vert_pos = -1;
            if (curr_item.Side == ChessSide.white && curr_item.Pos.vertical == 0 && prev_item.Pos.vertical == 0)
                vert_pos = 0;
            else if (curr_item.Side == ChessSide.black && curr_item.Pos.vertical == BoardKit.Max_Y - 1 && prev_item.Pos.vertical == BoardKit.Max_Y - 1)
                vert_pos = BoardKit.Max_Y - 1;

            if (vert_pos != -1)
            {
                bool do_castling = false;
                BoardPosition from1 = BoardPosition.None, from2 = BoardPosition.None;
                BoardPosition to1 = BoardPosition.None, to2 = BoardPosition.None;
                if (prev_item.Type == (byte)ClassicChessItemType.king && prev_item.Pos.horizontal == 4 && curr_item.Type == (byte)ClassicChessItemType.rook)
                {
                    if (curr_item.Pos.horizontal == 0)
                    {
                        do_castling = true;
                        from1 = prev_item.Pos;
                        to1 = new BoardPosition(2, vert_pos);
                        from2 = curr_item.Pos;
                        to2 = new BoardPosition(3, vert_pos);
                    }
                    else if (curr_item.Pos.horizontal == BoardKit.Max_X - 1)
                    {
                        do_castling = true;
                        from1 = prev_item.Pos;
                        to1 = new BoardPosition(BoardKit.Max_X - 2, vert_pos);
                        from2 = curr_item.Pos;
                        to2 = new BoardPosition(5, vert_pos);
                    }
                }
                else if (prev_item.Type == (byte)ClassicChessItemType.rook && curr_item.Type == (byte)ClassicChessItemType.king)
                {
                    if (prev_item.Pos.horizontal == 0)
                    {
                        do_castling = true;
                        from1 = prev_item.Pos;
                        to1 = new BoardPosition(3, vert_pos);
                        from2 = curr_item.Pos;
                        to2 = new BoardPosition(2, vert_pos);
                    }
                    else if (prev_item.Pos.horizontal == BoardKit.Max_X - 1)
                    {
                        do_castling = true;
                        from1 = prev_item.Pos;
                        to1 = new BoardPosition(5, vert_pos);
                        from2 = curr_item.Pos;
                        to2 = new BoardPosition(BoardKit.Max_X - 2, vert_pos);
                    }
                }

                if (do_castling)
                {
                    PreviousSelectedPosition = from1;
                    CurrentSelectedPosition = to1;
                    BoardKit.SetFigureOnBoard(to1, BoardKit.GetFigureFromBoard(from1));
                    if (backInTime)
                        BoardKit.GetFigureFromBoard(to1).Steps -= 2;
                    LastPlayerAction = PlayerAction.Move;
                    if (send_events)
                        OnChanged_Invoke(PreviousPlayer.Side);
                    PreviousSelectedPosition = from2;
                    CurrentSelectedPosition = to2;
                    BoardKit.SetFigureOnBoard(to2, BoardKit.GetFigureFromBoard(from2));
                    if (backInTime)
                        BoardKit.GetFigureFromBoard(to2).Steps -= 2;
                    LastPlayerAction = PlayerAction.Move;
                    if (send_events)
                        OnChanged_Invoke(PreviousPlayer.Side);

                    CheckForCheckAndMate(send_events);
                }
            }
        }
    }
}