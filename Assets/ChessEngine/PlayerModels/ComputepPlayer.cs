using System.Threading;
using System.Collections.Generic;
using System;

namespace ChessEngine
{
    public sealed class ComputerPlayer : PlayerBase
    {
        private byte skill;
        private IChessUCIEngine chessUCIEngine;

        public ComputerPlayer(ChessSide side, byte skill, IChessUCIEngine chessUCIEngine) : base(PlayerType.computer, side)
        {
            this.skill = skill;
            this.chessUCIEngine = chessUCIEngine;
            this.chessUCIEngine.StartEngine();
            this.chessUCIEngine.SetSkill(skill);
        }

        public override void DoAction(BoardPosition bp, bool back = false)
        {
            //var cmm_classic = fen as ClassicChessMatchModel;
            string captureOnTheIsle = UciConverter.BoardPositionToString(FEN.CaptureOnTheIsle);

            string fen = FEN.GetFEN();

            chessUCIEngine.GoInfinite(fen);

            SearchForMove();

        }

        

        private void SearchForMove()
        {
            double secs = DateTime.Now.ToOADate() * ChessEngineConstants.SecondsInDay;
            double new_secs = secs;
            while (new_secs - secs < (double)skill * 0.5)
            {
                /*Thinking*/
                new_secs = DateTime.Now.ToOADate() * ChessEngineConstants.SecondsInDay;
            }
            chessUCIEngine.Stop();

            secs = DateTime.Now.ToOADate() * ChessEngineConstants.SecondsInDay;
            new_secs = secs;
            bool gotMove = false;
            bool isMate = false;
            while (!gotMove)
            {
                new_secs = DateTime.Now.ToOADate() * ChessEngineConstants.SecondsInDay;
                if (new_secs - secs > 0.5)
                {
                    secs = new_secs;
                    int k = 0;
                    var answers = chessUCIEngine.GetAnswers();
                    foreach (var answer in answers)
                    {
                        if (answer.Contains("bestmove"))
                        {
                            if (!(char.IsLetter(answer[9]) && char.IsDigit(answer[10]) && char.IsLetter(answer[11]) && char.IsDigit(answer[12])))
                            {
                                throw new System.Exception("Wrong bestmove message format: " + answer);
                            }

                            string move_cmd = new string(new char[]{ answer[9], answer[10], answer[11], answer[12] });
                            BoardPosition from_temp;
                            BoardPosition to_temp;
                            UciConverter.GetBoardPositionsFromMoveCommand(move_cmd, out from_temp, out to_temp);

                            IChessItemModelShort fig = FigureOnBoard.GetFigureByPosition(from_temp);

                            if (fig.IsNullObject || fig.Side != Side)
                            {
                                throw new Exception("Wrong figure placement!");
                            }

                            List<bool> temp;
                            var bps = FigureOnBoard.GetPossibleMoves(fig.Pos, out temp);
                            bool is_to_temp_legal = false;
                            foreach (var bp in bps)
                            {
                                if (bp == to_temp)
                                {
                                    is_to_temp_legal = true;
                                    break;
                                }
                            }
                            if (!is_to_temp_legal)
                            {
                                //throw new Exception("Move Is Not Possibile by chess rules!");
                            }


                            if (!answer.Contains("ponder"))
                                isMate = true;

                            gotMove = true;
                            UciConverter.GetBoardPositionsFromMoveCommand(move_cmd, out from, out to);
                            break;
                        }
                    }
                }
            }
            SetBPs(ChessMatchCurrentState.CurrentSelectedPosition, from);
            Select();

            SetBPs(from, to);
            Move();


        }
    }
}
