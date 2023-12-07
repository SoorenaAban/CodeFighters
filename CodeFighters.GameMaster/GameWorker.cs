using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFighters.Models;
using CodeFighters.Data;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace CodeFighters.GameMaster
{
    public class GameWorker
    {
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
        private ScriptEngine engine;
        private ScriptScope scope;
        private DateTime TurnStartedAt { get; set; }

        public GameWorker(GameModel gameModel, ApiContext apiContext)
        {
            Game = gameModel;
            _apiContext = apiContext;

            engine = Python.CreateEngine();
            scope = engine.CreateScope();

            _playerOneReturnMessage = "";
            _playerTwoReturnMessage = "";
        }

        private void UpdateDatabase()
        {
            _apiContext.Update(Game);
            _apiContext.SaveChanges();
        }

        public void SubmitPlayerAction(string input, bool isPlayerOne)
        {

            var rightAnswer = "c";
            var wrongAnswer = "w";

            if (isPlayerOne && Game.PlayerOneAnswered && (input.ToLower().Contains(rightAnswer) || input.ToLower().Contains(wrongAnswer)))
            {
                _playerOneReturnMessage += "Player One already answered\n";
                return;
            }

            if (!isPlayerOne && Game.PlayerTwoAnswered && (input.ToLower().Contains(rightAnswer) || input.ToLower().Contains(wrongAnswer)))
            {
                _playerTwoReturnMessage += "Player Two already answered\n";
                return;
            }

            //C is the placeholder answer for a correct answer
            if (input.ToLower().Contains(rightAnswer))
            {
                if (isPlayerOne)
                {
                    Game.PlayerOneAnswered = true;
                    Game.PlayerOneHealth -= 1;

                    _playerOneReturnMessage += "Correct Answer\n";
                    _playerTwoReturnMessage += "Correct Answer for player one\n";
                }
                else
                {
                    Game.PlayerTwoAnswered = true;
                    Game.PlayerTwoHealth -= 1;

                    _playerTwoReturnMessage += "Correct Answer\n";
                    _playerOneReturnMessage += "Correct Answer for player two\n";
                }
            }

            //W is the placeholder answer for a wrong answer
            if (input.ToLower().Contains(wrongAnswer))
            {
                if (isPlayerOne)
                {
                    Game.PlayerOneAnswered = true;
                    Game.PlayerTwoHealth -= 1;

                    _playerOneReturnMessage += "Wrong Answer\n";
                    _playerTwoReturnMessage += "Wrong Answer for player one\n";
                }
                else
                {
                    Game.PlayerTwoAnswered = true;
                    Game.PlayerOneHealth -= 1;

                    _playerTwoReturnMessage += "Wrong Answer\n";
                    _playerOneReturnMessage += "Wrong Answer for player two\n";
                }
            }

            if (input.ToLower().Contains("ready"))
            {
                if (isPlayerOne)
                {
                    Game.PlayerOneReady = true;
                    _playerOneReturnMessage += "Player One Ready\n";
                    _playerOneReturnMessage += "Player One Ready\n";
                }
                else
                {
                    Game.PlayerTwoReady = true;
                    _playerTwoReturnMessage += "Player Two Ready\n";
                    _playerOneReturnMessage += "Player Two Ready\n";
                }
            }
        }

        public void GetGameValue(string value, bool isPlayerOne)
        {
            string temp = "\n";

            switch (value)
            {
                case "health":
                    temp += "Player One Health:" + Game.PlayerOneHealth.ToString() + "\n";
                    temp += "Player Two Health:" + Game.PlayerTwoHealth.ToString() + "\n";
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

            if(isPlayerOne)
                _playerOneReturnMessage += temp;
            else
                _playerTwoReturnMessage += temp;
        }

        public bool IsGameOver() 
        {
            if (Game.PlayerOneHealth <= 0 || Game.PlayerTwoHealth <= 0)
                return true;

            if (Game.TurnNumber >= Game.QuestionCount)
                return true;

            return false;
        }

        public int GetWinner()
        {
            if (Game.PlayerOneHealth <= 0 && Game.PlayerTwoHealth <= 0)
                return 4;

            if (Game.PlayerOneHealth <= 0)
                return 3;

            if (Game.PlayerTwoHealth <= 0)
                return 2;

            return 1;
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
                        _playerOneReturnMessage += "Game Aborted\n";
                        _playerTwoReturnMessage += "Game Aborted\n";
                    }
                }

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
                    _playerOneReturnMessage += "Game Over\n";
                    _playerTwoReturnMessage += "Game Over\n";
                    switch (Game.Result)
                    {
                        case 2:
                            _playerOneReturnMessage += "Player One Won\n";
                            _playerTwoReturnMessage += "Player One Won\n";
                            break;
                        case 3:
                        _playerOneReturnMessage += "Player Two Won\n";
                        _playerTwoReturnMessage += "Player Two Won\n";
                            break;
                        case 4:
                        _playerOneReturnMessage += "Tie\n";
                        _playerTwoReturnMessage += "Tie\n";
                            break;

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
