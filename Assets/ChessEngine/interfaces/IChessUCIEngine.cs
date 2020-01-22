using System;

public interface IChessUCIEngine
{
    void StartEngine();
    void StopEngine();
    void Send_Command(string command);
    string[] GetAnswers();
    void GoInfinite(string fen);
    void Stop();

    void SetSkill(int skill);
    void SetThreads(int num_of_threads);
}
