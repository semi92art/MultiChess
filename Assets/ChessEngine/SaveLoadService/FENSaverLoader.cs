using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace ChessEngine
{
    public class FENSaverLoader : IGameLoaderSaverService
    {
        public readonly string FileSavesPath;
        public const string SaveFileExtention = ".txt";

        public FENSaverLoader(string fileSavesPath)
        {
            FileSavesPath = fileSavesPath;
        }

        public bool SaveGame(string file_path, List<FigureMove> moves, ChessGameType gameType)
        {
            var sb = new StringBuilder();
            sb.Append((int)gameType);
            sb.Append(";");

            foreach (var move in moves)
            {
                sb.Append(((ChessLetter)move.from_x).ToString());
                sb.Append(move.from_y.ToString());
                sb.Append((ChessLetter)move.to_x).ToString();
                sb.Append(move.to_y.ToString());
                sb.Append(';');
            }

            int k = file_path.Length - SaveFileExtention.Length;
            foreach (var char_symb in SaveFileExtention)
            {
                if (char_symb != file_path[k])
                {
                    file_path = file_path + SaveFileExtention;
                    break;
                }
                k++;
            }

            try
            {
                var sw = File.CreateText(file_path);
                sw.Write(sb.ToString());
                sw.Close();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public bool LoadGame(string filePath, out ChessGameType gameType, out List<BoardPosition> fromPositions, out List<BoardPosition> toPositions)
        {
            string start_data_str = null;
            gameType = ChessGameType.classic;
            fromPositions = new List<BoardPosition>();
            toPositions = new List<BoardPosition>();
            try
            {
                if (File.Exists(filePath))
                    start_data_str = File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                return false;
            }

            var start_data = start_data_str.Split(';');

            gameType = (ChessGameType)int.Parse(start_data[0]);


            for (int i = 1; i < start_data.Length; i++)
            {
                if (!string.IsNullOrEmpty(start_data[i]))
                {
                    BoardPosition from;
                    BoardPosition to;
                    UciConverter.GetBoardPositionsFromMoveCommand(start_data[i], out from, out to);
                    fromPositions.Add(from);
                    toPositions.Add(to);
                }
            }
            return true;
        }
    }
}
