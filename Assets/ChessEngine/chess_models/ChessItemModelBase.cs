using System.Collections.Generic;
using System.Text;

namespace ChessEngine
{
    public abstract class ChessItemModelBase : IChessItemModel
    {
        public BoardPosition Pos { get; protected set; }
        public ChessSide Side { get; protected set; }
        public byte Type { get; set; }
        public int Steps { get; set; }
        public bool IsNullObject { get; protected set; }

        public abstract List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpLis, out List<bool> killPossibilityList);

        public ChessItemModelBase(ChessSide side, byte type, BoardPosition pos, int steps, bool isNullObject = false)
        {
            Side = side;
            Type = type;
            Pos = pos;
            Steps = steps;
            IsNullObject = isNullObject;
        }

        public void SetPosition(BoardPosition pos)
        {
            Steps++;
            Pos = pos;
        }

        public override string ToString()
        {
            if (Pos == BoardPosition.None)
                return "-";
            char letter;
            StringBuilder sb = new StringBuilder();
            if (Side == ChessSide.white)
                letter = (char)((char)48 + (char)Pos.horizontal + (char)17);
            else
                letter = (char)((char)48 + (char)Pos.horizontal + (char)49);
            sb.Append(letter);
            sb.Append(Pos.vertical);
            return sb.ToString();
        }
    }
}
