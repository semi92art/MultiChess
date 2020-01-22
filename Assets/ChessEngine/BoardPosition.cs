
namespace ChessEngine
{
    public struct BoardPosition
    {
        public byte horizontal;
        public byte vertical;
        public bool IsNullObject;
        public static BoardPosition None { get { return new BoardPosition(225, 225, true); } }

        public BoardPosition(byte horizontal, byte vertical, bool isNullObject = false)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
            this.IsNullObject = isNullObject;
        }

        public BoardPosition(int horizontal, int vertical, bool isNullObject = false)
        {
            this.horizontal = (byte)horizontal;
            this.vertical = (byte)vertical;
            this.IsNullObject = isNullObject;
        }

        public override string ToString()
        {
            return '(' + horizontal.ToString() + ';' + vertical.ToString() + ')';
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType())
                return false;
            else
            {
                BoardPosition temp = (BoardPosition)obj;
                return (temp.horizontal == horizontal && temp.vertical == vertical);
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(BoardPosition pos1, BoardPosition pos2)
        {
            return pos1.horizontal == pos2.horizontal && pos1.vertical == pos2.vertical;
        }

        public static bool operator !=(BoardPosition pos1, BoardPosition pos2)
        {
            return pos1.horizontal != pos2.horizontal || pos1.vertical != pos2.vertical;
        }
    }
}
