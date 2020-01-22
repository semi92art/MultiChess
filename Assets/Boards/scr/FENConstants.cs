using ChessEngine;

public static class FENConstants
{
    public enum ClassicFEN
    {
        Default,
        Endshpile_1
    }

    public enum LosAlamosFEN
    {
        Default,
        Endshpile_1
    }
    public enum ChaturangaFEN
    {
        Default
    }

    public enum CircledFEN
    {
        Default
    }



    public static readonly string FEN_Classic_Endshpile_1 = "6k1/8/7R/5Q2/2B5/B7/8/4K3 w - - 0 0";
    public static readonly string FEN_LosAlamos_Endshpile_1 = "4k1/6/5R/3Q2/2B3/B5/6/2K3 w";

    public static string GetClassicFEN(ClassicFEN fen)
    {
        switch (fen)
        {
            case ClassicFEN.Default:
                return ChessEngineConstants.FEN_Classic_Default;
            case ClassicFEN.Endshpile_1:
                return FEN_Classic_Endshpile_1;
            default:
                throw new System.NotImplementedException("GetClassicFEN was not implemented completely!");
        }
    }

    public static string GetLosAlamosFEN(LosAlamosFEN fen)
    {
        switch (fen)
        {
            case LosAlamosFEN.Default:
                return ChessEngineConstants.FEN_LosAlamos_Default;
            case LosAlamosFEN.Endshpile_1:
                return FEN_LosAlamos_Endshpile_1;
            default:
                throw new System.NotImplementedException("GetLosAlamosFEN was not implemented completely!");
        }
    }

    public static string GetChaturangaFEN(ChaturangaFEN fen)
    {
        switch (fen)
        {
            case ChaturangaFEN.Default:
                return ChessEngineConstants.FEN_Chaturanga_Default;
            default:
                throw new System.NotImplementedException("GetLosAlamosFEN was not implemented completely!");
        }
    }

    public static string GetCircledFEN(CircledFEN fen)
    {
        switch (fen)
        {
            case CircledFEN.Default:
                return ChessEngineConstants.FEN_Circled_Default;
            default:
                throw new System.NotImplementedException("GetCircledFEN was not implemented completely!");
        }
    }
}
