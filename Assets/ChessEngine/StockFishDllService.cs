using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ChessEngine
{
    public class StockFishDllService : IChessUCIEngine
    {
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartStockfish();
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void StopStockfish();
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetCommands(out IntPtr data, out int size);
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SendCommand([MarshalAs(UnmanagedType.LPStr)] string cmd);
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GoInfinite_Command([MarshalAs(UnmanagedType.LPStr)] string start_fen);
        [DllImport("stockfish", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Stop_Command();

        public void StartEngine()
        {
            StartStockfish();
        }

        public void StopEngine()
        {
            StopStockfish();
        }

        public void Send_Command(string cmd)
        {
            SendCommand(cmd);
        }

        public string[] GetAnswers()
        {
            IntPtr data;
            int size;
            int res = GetCommands(out data, out size);
            List<string> answers = new List<string>();
            if (size > 0)
            {
                string[] commands = ConvertBytesToUCICommands(data, size);
                foreach (var command in commands)
                {
                    if (!string.IsNullOrEmpty(command) && command != " ")
                        answers.Add(command);
                }
            }
            return answers.ToArray();
        }

        private static string[] ConvertBytesToUCICommands(IntPtr ptr, int size)
        {
            if (ptr.ToInt32() != 0 && size > 0)
            {
                byte[] byte_arr = new byte[size];
                Marshal.Copy(ptr, byte_arr, 0, size);
                string commands = Encoding.UTF8.GetString(byte_arr, 0, byte_arr.Length);
                return commands.Split(new string[] { "_CMD_" }, StringSplitOptions.None);
            }
            else
            {
                return null;
            }
        }

        public void GoInfinite(string fen)
        {
            GoInfinite_Command(fen);
        }

        public void Stop()
        {
            Stop_Command();
        }

        public void SetSkill(int skill)
        {
            if (skill < 0)
                skill = 0;
            else if (skill > 20)
                skill = 20;

            Send_Command("setoption name Skill Level value " + skill.ToString());
        }

        public void SetThreads(int num_of_threads)
        {
            if (num_of_threads < 1)
                num_of_threads = 1;
            else if (num_of_threads > 128)
                num_of_threads = 128;

            Send_Command("setoption name Threads value " + num_of_threads.ToString());
        }
    }
}
