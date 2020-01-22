using System;
using System.Collections.Generic;


namespace ChessEngine
{
    public abstract class PlayerBase : IPlayer
    {
        public PlayerType Type { get; private set; }
        public ChessSide Side { get; private set; }
        public IFigureOnBoard FigureOnBoard { get; private set; }
        public IFEN FEN { get; private set; }
        public IChessMatchCurrentState ChessMatchCurrentState { get; private set; }
        public IMessaging Messaging { get; private set; }


        protected BoardPosition from;
        protected BoardPosition to;

        public virtual void DoAction(BoardPosition bp, bool back = false)
        {
            var fig = FigureOnBoard.GetFigureByPosition(bp);
            if (fig.IsNullObject && ChessMatchCurrentState.CurrentSelectedPosition == BoardPosition.None)
                return;

            var prev_bp = ChessMatchCurrentState.PreviousSelectedPosition;
            var curr_bp = ChessMatchCurrentState.CurrentSelectedPosition;
            SetBPs(ChessMatchCurrentState.CurrentSelectedPosition, bp);

            IChessItemModelShort chess = FigureOnBoard.GetFigureByPosition(curr_bp);
            IChessItemModelShort prev_chess;
            if (prev_bp.horizontal < 0)
                prev_chess = new NullChessItemModelShort();
            else
                prev_chess = FigureOnBoard.GetFigureByPosition(curr_bp);


            //If there wasn't chosen any position on board
            if (prev_chess.IsNullObject)
            {
                //If there figure on this board position and it's side is current player's side
                if (!chess.IsNullObject && ChessMatchCurrentState.CurrentPlayer.Side == chess.Side)
                    Select();
            }
            //If one of the positions on board is chosen
            else
            {
                bool wasMoved = false;
                //Check for possible moves
                List<bool> temp;
                foreach (var possibleMove in FigureOnBoard.GetPossibleMoves(ChessMatchCurrentState.PreviousSelectedPosition, out temp))
                {
                    //If figure can move to this board position
                    if (possibleMove == ChessMatchCurrentState.CurrentSelectedPosition)
                    {
                        Move();
                        wasMoved = true;
                        break;
                    }
                }

                //If move was not possible
                if (!wasMoved)
                {
                    if (!chess.IsNullObject && chess.Side == prev_chess.Side)
                        Select();
                    else
                        SetBPs(prev_bp, curr_bp);
                }
            }
        }
        


        public PlayerBase(PlayerType playerType, ChessSide chessSide)
        {
            Type = playerType;
            Side = chessSide;
        }

        public void InitInterfaces(IMessaging messaging, IFigureOnBoard figureOnBoard, IFEN fen, IChessMatchCurrentState chessMatchCurrentState)
        {
            Messaging = messaging;
            FigureOnBoard = figureOnBoard;
            FEN = fen;
            ChessMatchCurrentState = chessMatchCurrentState;
        }

        protected void SetBPs(BoardPosition bp_from, BoardPosition bp_to)
        {
            int from_isnull = bp_from.IsNullObject ? 1 : 0;
            int to_isnull = bp_to.IsNullObject ? 1 : 0;
            Messaging.SendMessage("set_bps_" + bp_from.horizontal.ToString() + "_" + bp_from.vertical.ToString() + "_" + from_isnull.ToString() + "_" + bp_to.horizontal.ToString() + "_" + bp_to.vertical.ToString() + "_" + to_isnull.ToString());
        }

        protected void Select()
        {
            Messaging.SendMessage("select");
        }

        protected void Move()
        {
            Messaging.SendMessage("move");
        }
    }
}
