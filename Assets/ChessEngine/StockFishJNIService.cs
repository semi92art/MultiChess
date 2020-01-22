using System;
using UnityEngine;
using System.Collections.Generic;


namespace ChessEngine
{
    public class StockfishJNIService : IChessUCIEngine
    {
        private List<string> answers = new List<string>();
        private AndroidJavaObject stockfishEng;

        public StockfishJNIService()
        {
            InitializeStockfishEngine();
        }

        private void InitializeStockfishEngine()
        {
            stockfishEng = new AndroidJavaObject("space.space.chessenginenativeplugin.ChessEngineLibAPI");
        }

        public void StartEngine()
        {
            stockfishEng.Call("StartStockfish");
        }

        public void StopEngine()
        {
            stockfishEng.Call("StopStockfish");
        }

        public void Send_Command(string cmd)
        {
            stockfishEng.Call("SendCommand", cmd);
        }

        public string[] GetAnswers()
        {
            string unparsed_answers = stockfishEng.Call<string>("GetAnswers");
            string[] splitted_answers = unparsed_answers.Split(new string[] { "_CMD_" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var answer in splitted_answers)
            {
                answers.Add(answer);
            }
            return answers.ToArray();
        }

        public void GoInfinite(string fen)
        {
            stockfishEng.Call("UCIGoInfinite", fen);
        }

        public void Stop()
        {
            stockfishEng.Call("UCIStop");
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

