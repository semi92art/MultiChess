using System.Collections.Generic;

namespace ChessEngine
{
    public sealed class ClassicQueenModel : ChessItemModelBase
    {
        public ClassicQueenModel(ChessSide side, BoardPosition pos, int steps = 0) : base(side, (byte)ClassicChessItemType.queen, pos, steps) { }
        public override List<BoardPosition> GetPossibleMoves(IChessItemModel[,] items, out List<bool> isJumpList, out List<bool> killPossibilityList)
        {
            var result = new List<BoardPosition>();
            List<bool> isJumpList1;
            List<bool> isJumpList2;

            List<bool> killPossibilityList1;
            List<bool> killPossibilityList2;

            foreach (var poss_move in PossibleMovesFinder.GetRookPossibleMoves(items, Side, Pos, Steps, false, false, out isJumpList1, out killPossibilityList1))
            {
                result.Add(poss_move);
            }

            foreach (var poss_move in PossibleMovesFinder.GetBishopPossibleMoves(items, Side, Pos, false, out isJumpList2, out killPossibilityList2))
            {
                if (poss_move != Pos)
                    result.Add(poss_move);
            }

            isJumpList = new List<bool>();
            foreach (var isJump in isJumpList1)
                isJumpList.Add(isJump);
            foreach (var isJump in isJumpList2)
                isJumpList.Add(isJump);

            killPossibilityList = new List<bool>();
            foreach (var killPossibility in killPossibilityList1)
                killPossibilityList.Add(killPossibility);
            foreach (var killPossibility in killPossibilityList2)
                killPossibilityList.Add(killPossibility);

            return result;
        }
    }
}
