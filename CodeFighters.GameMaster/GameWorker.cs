using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFighters.Models;
using CodeFighters.Data;
using System.Diagnostics.CodeAnalysis;

namespace CodeFighters.GameMaster
{
    public class GameWorker
    {
        private bool _isVsAI;

        private string _playerOneReturnMessage;
        public string PlayerOneReturnMessage 
        { 
            get
            {
                string temp = _playerOneReturnMessage;
                _playerOneReturnMessage = "";
                return temp;
            }
            set
            {
                _playerOneReturnMessage = value;
            }
        }
        private string _playerTwoReturnMessage;
        public string PlayerTwoReturnMessage
        {
            get
            {
                string temp = _playerTwoReturnMessage;
                _playerTwoReturnMessage = "";
                return temp;
            }
            set
            {
                _playerOneReturnMessage = value;
            }
        }

        private const int TICKS = 60;
        private const int SECS_PER_TURN = 60;

        public GameModel Game { get; set; }
        private ApiContext _apiContext;
        private DateTime TurnStartedAt { get; set; }
        private GameCodeHost _gameCodeHost;

        public GameWorker(GameModel gameModel, ApiContext apiContext, GameCodeHost gameCodeHost)
        {
            Game = gameModel;
            _apiContext = apiContext;
            _gameCodeHost = gameCodeHost;


            _playerOneReturnMessage = "";
            _playerTwoReturnMessage = "";
            _gameCodeHost = gameCodeHost;
            _isVsAI = gameModel.IsVsAI;
        }

        private void UpdateDatabase()
        {
            _apiContext.Update(Game);
            _apiContext.SaveChanges();
        }

        private void SendMessage(string target, bool isPlayerOne, bool isBoth)
        {
            if(_isVsAI)
            {
                _playerOneReturnMessage += target + "\n";
                return;
            }

            if (isBoth)
            {
                _playerOneReturnMessage += target + "\n";
                _playerTwoReturnMessage += target + "\n";
            }
            else if (isPlayerOne)
            {
                _playerOneReturnMessage += target + "\n";
            }
            else
            {
                _playerTwoReturnMessage += target + "\n";
            }
        }

        public void SubmitPlayerAction(string input, bool isPlayerOne)
        {

            if (isPlayerOne && Game.PlayerOneAnswered)
            {
                SendMessage("Player One already answered", isPlayerOne, false);
                return;
            }

            if (!isPlayerOne && Game.PlayerTwoAnswered)
            {
                SendMessage("Player Two already answered", isPlayerOne, false);
                return;
            }

            
            if (input.ToLower().Contains("answer"))
            {
                throw new NotImplementedException();
                var asnwerIsCorrect = _gameCodeHost.SubmitAnswer(input, isPlayerOne ? "one" : "two");
                if (asnwerIsCorrect)
                {
                    if (isPlayerOne)
                    {
                        Game.PlayerOneAnswered = true;
                        Game.PlayerOneHealth -= 1;

                        SendMessage("Correct Answer for player one", isPlayerOne, true);
                    }
                    else
                    {
                        Game.PlayerTwoAnswered = true;
                        Game.PlayerTwoHealth -= 1;

                        SendMessage("Correct Answer for player two", isPlayerOne, true);
                    }
                }
            }
            else
            {
                if (isPlayerOne)
                {
                    Game.PlayerOneAnswered = true;
                    Game.PlayerTwoHealth -= 1;

                    SendMessage("Wrong Answer for player one", isPlayerOne, true);
                }
                else
                {
                    Game.PlayerTwoAnswered = true;
                    Game.PlayerOneHealth -= 1;

                    SendMessage("Wrong Answer for player two", isPlayerOne, true);
                }
            }

            if (input.ToLower().Contains("ready"))
            {
                if (isPlayerOne)
                {
                    Game.PlayerOneReady = true;
                    SendMessage("Player One Ready", isPlayerOne, true);
                }
                else
                {
                    Game.PlayerTwoReady = true;
                    SendMessage("Player Two Ready", isPlayerOne, true);
                }
            }
        }

        public void GetGameValue(string value, bool isPlayerOne)
        {
            string temp = "\n";

            switch (value)
            {
                case "health":
                    temp += "Player One Health:" + _gameCodeHost.GetHealth("health", "one").ToString() + "\n";
                    temp += "Player Two Health:" + _gameCodeHost.GetHealth("health", "two").ToString() + "\n";
                    break;
                case "turn":
                    temp += "Turn Number:" + Game.TurnNumber.ToString() + "\n";
                    break;
                case "time":
                    temp += "Time Created:" + Game.CreatedAt.ToString() + "\n";
                    temp += "Time Started:" + Game.StartTime.ToString() + "\n";
                    break;
                case "turntime":
                    temp += "Turn Started At:" + TurnStartedAt.ToString() + "\n";
                    temp += "Turn Ends At:" + TurnStartedAt.AddSeconds(SECS_PER_TURN).ToString() + "\n";
                    break;
                case "isready":
                    temp += "Player One Ready:" + Game.PlayerOneReady.ToString() + "\n";
                    temp += "Player Two Ready:" + Game.PlayerTwoReady.ToString() + "\n";
                    break;
            }

            SendMessage(temp, isPlayerOne, false);
        }

        public bool IsGameOver() 
        {

            return _gameCodeHost.IsGameOver();
        }

        public int GetWinner()
        {
            return _gameCodeHost.GetWinner();
        }

        public bool IsTurnOver()
        {
            if (Game.PlayerOneAnswered && Game.PlayerTwoAnswered)
                return true;

            if (TurnStartedAt.AddSeconds(SECS_PER_TURN) < DateTime.Now)
                return true;

            return false;
        }

        public void EndTurn()
        {

            Game.TurnNumber++;
            TurnStartedAt = DateTime.Now;
            Game.PlayerOneAnswered = false;
            Game.PlayerTwoAnswered = false;
            _gameCodeHost.SubmitTurnEnd();
            UpdateDatabase();
        }

        public void UserInput(string input, bool isPlayerOne)
        {
            if (input.ToLower().Contains("get"))
                GetGameValue(input.ToLower().Replace("get", "").Trim(), isPlayerOne);
            else SubmitPlayerAction(input, isPlayerOne);
        }

        public void Run()
        {
            Game.IsRunning = true;
            _gameCodeHost.StartGame(false);
            if (_isVsAI)
                Game.PlayerTwoReady = true;

            while (Game.IsRunning)
            {
                var start = DateTime.Now;

                if (!Game.HasStarted)
                {
                    if (Game.PlayerOneReady && Game.PlayerTwoReady)
                    {
                        Game.HasStarted = true;
                        Game.StartTime = DateTime.Now;
                        UpdateDatabase();
                    }

                    if (Game.CreatedAt.AddSeconds(GameModel.MAX_SEC_WAITING_FOR_PLAYERS) < DateTime.Now)
                    {
                        Game.IsRunning = false;
                        Game.EndTime = DateTime.Now;
                        UpdateDatabase();
                        SendMessage("Game Over", true, true);
                    }
                }

                else
                {

                    if (IsTurnOver())
                    {
                        EndTurn();
                    }

                    if (IsGameOver())
                    {
                        Game.Result = GetWinner();
                        Game.IsRunning = false;
                        Game.EndTime = DateTime.Now;
                        UpdateDatabase();
                        SendMessage("Game Over", true, true);
                        switch (Game.Result)
                        {
                            case 2:
                                SendMessage("Player One Won", true, true);
                                break;
                            case 3:
                                SendMessage("Player Two Won", true, true);
                                break;
                            case 4:
                                SendMessage("Tie", true, true);
                                break;

                        }
                    }
                }

                var end = DateTime.Now;
                var elapsed = end - start;
                var sleep = TICKS - elapsed.Milliseconds;
                if (sleep > 0)
                    Thread.Sleep(sleep);
            }

            Game.IsActive = false;
        }
    }
}
