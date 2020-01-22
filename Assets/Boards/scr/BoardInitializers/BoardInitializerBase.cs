using UnityEngine;
using ChessEngine;
using System.Collections.Generic;


public abstract class BoardInitializerBase : MonoBehaviour, IBoardInitializer
{
    protected PlayerFactory playerFactory;
    protected IPlayer[] players;
    protected bool initialized;

    protected void Start() { }

    public abstract IMatchController InitializeBoard();
    public abstract IMatchController InitializeBoard(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions);
    protected abstract IMatchController Init_Board(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions);

}

