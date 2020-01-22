using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class HumanPlayer : PlayerBase
    {
        public HumanPlayer(ChessSide side) : base(PlayerType.human, side) { }


        public override void DoAction(BoardPosition bp, bool back = false)
        {
            var fig = FigureOnBoard.GetFigureByPosition(bp);
            if (fig.IsNullObject && ChessMatchCurrentState.CurrentSelectedPosition == BoardPosition.None)
                return;

            var prev_bp = ChessMatchCurrentState.PreviousSelectedPosition;
            var curr_bp = ChessMatchCurrentState.CurrentSelectedPosition;
            SetBPs(ChessMatchCurrentState.CurrentSelectedPosition, bp);

            IChessItemModelShort chess = FigureOnBoard.GetFigureByPosition(ChessMatchCurrentState.CurrentSelectedPosition);
            IChessItemModelShort prev_chess;
            if (ChessMatchCurrentState.PreviousSelectedPosition.IsNullObject)
                prev_chess = new NullChessItemModelShort();
            else
                prev_chess = FigureOnBoard.GetFigureByPosition(ChessMatchCurrentState.PreviousSelectedPosition);

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
                var possibleMoves = new List<BoardPosition>();
                List<bool> temp;
                if (!back)
                    possibleMoves = FigureOnBoard.GetPossibleMoves(ChessMatchCurrentState.PreviousSelectedPosition, out temp);
                else
                {
                    for (byte i = 0; i < FigureOnBoard.BoardSizeX; i++)
                        for (byte j = 0; j < FigureOnBoard.BoardSizeY; j++)
                            possibleMoves.Add(new BoardPosition(i, j));
                }


                foreach (var possibleMove in possibleMoves)
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
    }
}
