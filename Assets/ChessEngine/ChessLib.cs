using System;


namespace ChessEngine
{
    

    public enum Direction : byte
    {
        top,
        bottom,
        left,
        right
    }

    public static class ChessEngineFunctions
    {
        public static IChessItemModel GetKilledChessItemFromMove(FigureMove move, ChessGameType chessType)
        {
            var side = (ChessSide)move.killed_side;
            var bp = new BoardPosition(move.killed_x, move.killed_y);
            switch (chessType)
            {
                case ChessGameType.classic:
                    var type_classic = (ClassicChessItemType)move.killed_type;
                    switch (type_classic)
                    {
                        case ClassicChessItemType.pawn:
                            return new ClassicPawnModel(side, bp, move.killed_steps);
                        case ClassicChessItemType.rook:
                            return new ClassicRookModel(side, bp, move.killed_steps);
                        case ClassicChessItemType.knight:
                            return new ClassicKnightModel(side, bp, move.killed_steps);
                        case ClassicChessItemType.bishop:
                            return new ClassicBishopModel(side, bp, move.killed_steps);
                        case ClassicChessItemType.queen:
                            return new ClassicQueenModel(side, bp, move.killed_steps);
                        case ClassicChessItemType.king:
                            return new ClassicKingModel(side, bp, move.killed_steps);
                        default:
                            throw new ArgumentException("Figire Move Has Wrong Format!");
                    }
                case ChessGameType.los_alamos:
                    var type_los_alamos = (LosAlamosChessItemType)move.killed_type;
                    switch (type_los_alamos)
                    {
                        case LosAlamosChessItemType.pawn:
                            return new LosAlamosPawnModel(side, bp, move.killed_steps);
                        case LosAlamosChessItemType.rook:
                            return new LosAlamosRookModel(side, bp, move.killed_steps);
                        case LosAlamosChessItemType.knight:
                            return new LosAlamosKnightModel(side, bp, move.killed_steps);
                        case LosAlamosChessItemType.queen:
                            return new LosAlamosQueenModel(side, bp, move.killed_steps);
                        case LosAlamosChessItemType.king:
                            return new LosAlamosKingModel(side, bp, move.killed_steps);
                        default:
                            throw new ArgumentException("Figire Move Has Wrong Format!");
                    }
                case ChessGameType.chaturanga:
                    var type_chaturanga = (ChaturangaChessItemType)move.killed_type;
                    switch (type_chaturanga)
                    {
                        case ChaturangaChessItemType.pawn:
                            return new ChaturangaPawnModel(side, bp, move.killed_steps);
                        case ChaturangaChessItemType.rook:
                            return new ChaturangaRookModel(side, bp, move.killed_steps);
                        case ChaturangaChessItemType.knight:
                            return new ChaturangaKnightModel(side, bp, move.killed_steps);
                        case ChaturangaChessItemType.king:
                            return new ChaturangaKingModel(side, bp, move.killed_steps);
                        default:
                            throw new ArgumentException("Figire Move Has Wrong Format!");
                    }
                case ChessGameType.circled:
                    var type_circled = (CircledChessItemType)move.killed_type;
                    switch (type_circled)
                    {
                        case CircledChessItemType.pawn_left:
                            return new CircledPawnModel(side, bp, Direction.left, move.killed_steps);
                        case CircledChessItemType.pawn_right:
                            return new CircledPawnModel(side, bp, Direction.right, move.killed_steps);
                        case CircledChessItemType.rook:
                            return new CircledRookModel(side, bp, move.killed_steps);
                        case CircledChessItemType.knight:
                            return new CircledKnightModel(side, bp, move.killed_steps);
                        case CircledChessItemType.king:
                            return new CircledKingModel(side, bp, move.killed_steps);
                        default:
                            throw new ArgumentException("Figire Move Has Wrong Format!");
                    }
                default:
                    throw new NotImplementedException("GetKilledChessItemFromMove Not Implemented Completely!");
            }


            throw new ArgumentException("Figire Move Has Wrong Format!");
        }
    }

    

    public enum TimerType
    {
        bullet_3min,
        blitz_3_10min,
        rapid_10_60_min,
        unlimited
    }

    public enum PlayerType
    {
        human,
        computer
    }

    public enum ChessSide
    {
        white = 0,
        black,
        red,
        green
    }

    public enum MatchStage
    {
        not_started,
        regular,
        check,
        mate
    }


    public enum Castling
    {
        _none,
        _short,
        _long,
        _short_and_long,
        _done
    }

    public enum SkillLevel
    {
        level_1 = 1,
        level_2,
        level_3,
        level_4,
        level_5,
        level_6,
        level_7,
        level_8,
        level_9,
        level_10,
        level_11,
        level_12,
        level_13,
        level_14,
        level_15,
        level_16,
        level_17,
        level_18,
        level_19,
        level_20
    }

}



