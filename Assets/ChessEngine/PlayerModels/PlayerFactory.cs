

namespace ChessEngine
{
    public class PlayerFactory
    {
        public IPlayer CreateHumanPlayer(ChessSide side)
        {
            return new HumanPlayer(side);
        }

        public IPlayer CreateComputerPlayer(ChessSide side, SkillLevel skillLevel, IChessUCIEngine uciEngine)
        {
            return new ComputerPlayer(side, (byte)skillLevel, uciEngine);
        }
    }
}
