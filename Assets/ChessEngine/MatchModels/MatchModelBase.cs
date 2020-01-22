using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessEngine
{
    public abstract class MatchModelBase : IChessMatchCurrentState, IFEN, IMoveBack, IChessAction, IFigureOnBoard, IMessaging, ICheckMate, IGameSaver
    {
        public event EventHandler<ChessSideArgs> OnChanged = (sender, e) => {};
        public event EventHandler<ChessSideArgs> OnCheck = (sender, e) => { };
        public event EventHandler<ChessSideArgs> OnMate = (sender, e) => { };
        public event EventHandler<ChessItemArgs> OnFigureRecovery = (sender, e) => { };

        public PlayerAction LastPlayerAction { get; protected set; }
        
        public IPlayer CurrentPlayer { get; protected set; }
        public IPlayer PreviousPlayer { get; protected set; }
        public BoardPosition CurrentSelectedPosition { get; protected set; }
        public BoardPosition PreviousSelectedPosition { get; protected set; }
        public bool IsJump { get; protected set; }
        public List<FigureMove> AllMoves { get; protected set; }
        public MatchStage Match_Stage { get; protected set; }

        public IChessBoardKit BoardKit;
        public ChessGameType chessGameType;
        protected List<IPlayer> Players;
        protected int currentPlayerIndex;

        protected IGameLoaderSaverService gameLoaderSaverService;

        #region FEN Data

        public bool WasCaptureOnTheIsle { get; protected set; }
        public BoardPosition CaptureOnTheIsle { get; protected set; }
        public int NumberOfReversibleSemiSteps { get; protected set; }
        public int TotalSteps { get; protected set; }
        public Castling WhiteCastling { get; protected set; }
        public Castling BlackCastling { get; protected set; }
        public byte BoardSizeX { get { return BoardKit.Max_X; } }
        public byte BoardSizeY { get { return BoardKit.Max_Y; } }

        public virtual string GetFEN()
        {
            return UciConverter.ConvertToFEN(BoardKit.FiguresPlacement, CurrentPlayer.Side, WhiteCastling, BlackCastling, CaptureOnTheIsle.ToString(), NumberOfReversibleSemiSteps, TotalSteps);
        }

        #endregion


        public virtual List<BoardPosition> GetPossibleMoves(BoardPosition bp, out List<bool> isJumpList)
        {
            isJumpList = new List<bool>();
            var res = new List<BoardPosition>();
            if (!BoardKit.GetFigureFromBoard(bp).IsNullObject)
            {
                List<bool> isJumpListTemp;
                List<bool> killPossibilityList;
                int k = -1;
                var poss_moves = BoardKit.GetFigureFromBoard(bp).GetPossibleMoves(BoardKit.FiguresPlacement, out isJumpListTemp, out killPossibilityList);
                foreach (var poss_move in poss_moves)
                {
                    k++;
                    bool isNewPositionEmpty = BoardKit.FiguresPlacement[poss_move.horizontal, poss_move.vertical].IsNullObject;

                    if (!isNewPositionEmpty && !killPossibilityList[k])
                        continue;

                    if (poss_move != bp)
                    {
                        var temp_chess_item = BoardKit.MoveChessItemPredictForward(bp, poss_move);
                        var chess_item_setted = BoardKit.GetFigureFromBoard(poss_move);
                        if (!BoardKit.CheckForCheck(chess_item_setted.Side) )
                        {
                            res.Add(poss_move);
                            isJumpList.Add(isJumpListTemp[k]);
                        }
                        BoardKit.MoveChessItemPredictBackward(temp_chess_item, bp, poss_move);
                    }
                }
            }
            return res;
        }

        protected virtual void TryMove(bool backInTime = false, bool send_events = true)
        {
            BoardPosition captureOnTheIsle;
            IChessItemModel killed_chess;
            bool tryMoveResult = BoardKit.TryMoveChessItem(CurrentPlayer.Side, PreviousSelectedPosition, CurrentSelectedPosition, out captureOnTheIsle, out killed_chess);

            if (tryMoveResult)
            {
                AllMoves.Add(new FigureMove(PreviousSelectedPosition, CurrentSelectedPosition, killed_chess, CurrentPlayer.Side));

                PreviousPlayer = Players[currentPlayerIndex];
                if (++currentPlayerIndex >= Players.Count)
                    currentPlayerIndex = 0;
                CurrentPlayer = Players[currentPlayerIndex];
            }

            //If move is ready
            if (tryMoveResult)
            {
                var prev_item = BoardKit.GetFigureFromBoard(PreviousSelectedPosition);
                var curr_item = BoardKit.GetFigureFromBoard(CurrentSelectedPosition);


                if (!prev_item.IsNullObject && !curr_item.IsNullObject)
                {
                    LastPlayerAction = PlayerAction.Move;
                    if (send_events)
                        OnChanged_Invoke(PreviousPlayer.Side);
                }
                else
                {
                    LastPlayerAction = PlayerAction.MoveAndKill;
                    if (send_events)
                        OnChanged_Invoke(PreviousPlayer.Side);
                }

                CurrentSelectedPosition = BoardPosition.None;
                PreviousSelectedPosition = BoardPosition.None;
                if (send_events)
                    CheckForCheckAndMate(send_events);
            }
            else
            {
                CurrentSelectedPosition = PreviousSelectedPosition;
                PreviousSelectedPosition = BoardPosition.None;
            }
        }

        public virtual void MakeAction(BoardPosition pos)
        {
            CurrentPlayer.DoAction(pos);
        }

        public virtual void SendMessage(string message)
        {
            if (message.Contains("set_bps"))
            {
                message = message.Replace("set_bps_", "");
                var msg_spl = message.Split('_');
                bool isNull_from = msg_spl[2] == "1";
                bool isNull_to = msg_spl[5] == "1";
                PreviousSelectedPosition = new BoardPosition(int.Parse(msg_spl[0].ToString()), int.Parse(msg_spl[1].ToString()), isNull_from);
                CurrentSelectedPosition = new BoardPosition(int.Parse(msg_spl[3].ToString()), int.Parse(msg_spl[4].ToString()), isNull_to);
            }
            if (message.Contains("select"))
            {
                LastPlayerAction = PlayerAction.Select;
                OnChanged_Invoke(PreviousPlayer.Side);
            }
            else if (message.Contains("move"))
            {
                TryMove();
            }
        }
        
        public virtual void MoveBack()
        {
            if (AllMoves.Count == 0)
                return;

            var move = AllMoves.Last();

            PreviousPlayer = Players[currentPlayerIndex];
            currentPlayerIndex--;
            if (currentPlayerIndex < 0)
                currentPlayerIndex = Players.Count - 1;
            

            LastPlayerAction = PlayerAction.Nothing;


            PreviousSelectedPosition = new BoardPosition(move.to_x, move.to_y);
            CurrentSelectedPosition = new BoardPosition(move.from_x, move.from_y, true);

            BoardKit.SetFigureOnBoard(CurrentSelectedPosition, BoardKit.GetFigureFromBoard(PreviousSelectedPosition));
            BoardKit.GetFigureFromBoard(CurrentSelectedPosition).Steps -= 2;

            var chessFactory = new ChessItemModelFactory();
            IChessItemModel killed_chess;
            if (move.killed_isnull)
                killed_chess = new NullChessItemModel();
            else
                killed_chess = chessFactory.CreateChessItemModel(new BoardPosition(move.killed_x, move.killed_y), move.killed_type, move.killed_steps, move.killed_side);

            BoardKit.FiguresPlacement[PreviousSelectedPosition.horizontal, PreviousSelectedPosition.vertical] = killed_chess;

            IChessItemModel recoveryItem = null;
            if (!move.killed_isnull)
            {
                recoveryItem = ChessEngineFunctions.GetKilledChessItemFromMove(move, chessGameType);
                BoardKit.FiguresPlacement[move.killed_x, move.killed_y] = recoveryItem;
                
            }

            AllMoves.RemoveAt(AllMoves.Count - 1);
            LastPlayerAction = PlayerAction.Move;
            CurrentPlayer = Players[currentPlayerIndex];
            OnChanged(this, new ChessSideArgs(move.side));

            if (!move.killed_isnull)
                OnFigureRecovery(this, new ChessItemArgs(recoveryItem));
        }

        protected void OnChanged_Invoke(ChessSide side)
        {
            OnChanged(this, new ChessSideArgs(side));
        }

        protected void OnCheck_Invoke(ChessSide side)
        {
            OnCheck(this, new ChessSideArgs(side));
        }

        protected void OnMate_Invoke(ChessSide side)
        {
            OnMate(this, new ChessSideArgs(side));
        }

        public ChessItemModelShort GetFigureByPosition(BoardPosition bp)
        {
            var chess_item = BoardKit.GetFigureFromBoard(bp);
            return new ChessItemModelShort(chess_item.Side, chess_item.Pos, chess_item.Type, chess_item.Steps, chess_item.IsNullObject);
        }

        public MatchModelBase(string fen, List<BoardPosition> fromPositions, List<BoardPosition> toPositions, List<IPlayer> playerList, ChessGameType chessGameType_, IGameLoaderSaverService gameLoaderSaverService_)
        {
            if (fen == null)
                throw new NullReferenceException("FEN String Was Not Set!");
            if (playerList == null)
                throw new NullReferenceException("Player List Was Not Set!");

            bool isAnyPlayerNotImplemented = false;
            foreach (var player in playerList)
                if (player == null)
                {
                    isAnyPlayerNotImplemented = true;
                    break;
                }

            if (isAnyPlayerNotImplemented)
                throw new NullReferenceException("One Or More Players In List Were Not Set!");
            if (fromPositions == null)
                throw new NullReferenceException("From-Positions Is Null!");
            if (toPositions == null)
                throw new NullReferenceException("To-Positions Is Null!");
            if (gameLoaderSaverService_ == null)
                throw new NullReferenceException("GameLoaderSaverService Not Set!");

            Players = playerList;
            chessGameType = chessGameType_;
            gameLoaderSaverService = gameLoaderSaverService_;

            this.PreviousSelectedPosition = BoardPosition.None;
            this.CurrentSelectedPosition = BoardPosition.None;

            CurrentPlayer = playerList[0];
            PreviousPlayer = playerList.Last();

            foreach (var player in playerList)
                player.InitInterfaces(this, this, this, this);


            
        }

        protected virtual void SetBoardByPositionMoves(List<BoardPosition> fromPositions, List<BoardPosition> toPositions)
        {
            AllMoves = new List<FigureMove>();
            for (int i = 0; i < fromPositions.Count; i++)
            {
                LastPlayerAction = PlayerAction.Select;
                PreviousSelectedPosition = fromPositions[i];
                CurrentSelectedPosition = toPositions[i];

                TryMove(false, false);
            }
        }

        protected virtual void CheckForCheckAndMate(bool send_events)
        {
            if (BoardKit.CheckForMate(CurrentPlayer.Side))
            {
                Match_Stage = MatchStage.mate;
                if (send_events)
                    OnMate_Invoke(PreviousPlayer.Side);
            }
            else
            {
                if (BoardKit.CheckForCheck(CurrentPlayer.Side))
                {
                    Match_Stage = MatchStage.check;
                    if (send_events)
                        OnCheck_Invoke(CurrentPlayer.Side);
                        //OnCheck_Invoke(PreviousPlayer.Side);
                }
            }
        }

        public bool SaveGame(string file_name)
        {
            return gameLoaderSaverService.SaveGame(file_name, AllMoves, chessGameType);
        }

        
    }
}
