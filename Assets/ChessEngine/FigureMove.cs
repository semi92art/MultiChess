using System.Text;

namespace ChessEngine
{
    public struct FigureMove
    {
        public byte from_x;
        public byte from_y;
        public byte to_x;
        public byte to_y;
        public byte killed_type;
        public ChessSide killed_side;
        public short killed_steps;
        public byte killed_x;
        public byte killed_y;
        public bool killed_isnull;
        public ChessSide side;

        public FigureMove(BoardPosition from, BoardPosition to, IChessItemModel killed_item, ChessSide side)
        {
            from_x = from.horizontal;
            from_y = from.vertical;
            to_x = to.horizontal;
            to_y = to.vertical;
            killed_side = killed_item.Side;
            killed_type = killed_item.Type;
            killed_steps = (short)killed_item.Steps;
            killed_x = killed_item.Pos.horizontal;
            killed_y = killed_item.Pos.vertical;
            killed_isnull = killed_item.IsNullObject;
            this.side = side;
        }

        public FigureMove(string s)
        {
            var split = s.Split('_');
            from_x = byte.Parse(split[0]);
            from_y = byte.Parse(split[1]);
            to_x = byte.Parse(split[2]);
            to_y = byte.Parse(split[3]);
            killed_type = byte.Parse(split[4]);
            killed_side = (ChessSide)byte.Parse(split[5]);
            killed_steps = short.Parse(split[6]);
            killed_x = byte.Parse(split[7]);
            killed_y = byte.Parse(split[8]);
            killed_isnull = byte.Parse(split[8]) == 1;
            this.side = (ChessSide)byte.Parse(split[9]);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(from_x);
            sb.Append('_');
            sb.Append(from_y);
            sb.Append('_');
            sb.Append(to_x);
            sb.Append('_');
            sb.Append(to_y);
            sb.Append('_');
            sb.Append(killed_type);
            sb.Append('_');
            sb.Append(killed_side);
            sb.Append('_');
            sb.Append(killed_steps);
            sb.Append('_');
            sb.Append(killed_x);
            sb.Append('_');
            sb.Append(killed_y);
            return sb.ToString();
        }
    }
}
