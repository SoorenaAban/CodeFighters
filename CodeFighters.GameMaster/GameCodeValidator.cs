using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeFighters.Models;
using IronPython.Compiler.Ast;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace CodeFighters.GameMaster
{
    public class GameCodeValidator
    {
        string _gameCode;
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private dynamic _game;
        public string ErrorMessage { get; private set; }

        public GameCodeValidator(string gameCode)
        {
            ErrorMessage = string.Empty;
            _gameCode = gameCode;   
        }

        public bool Validate()
        {
            if( !ValidateStart() ||
                !ValidateAnswer() ||
                !ValidateTurn() ||
                !ValidateGameOver() ||
                !ValidateWinner() ||
                !ValidateGetHealth())
            {
                return false;
            }
            return true;
        }

        public bool ValidateStart()
        {
            try
            {
                _engine = Python.CreateEngine();
                _scope = _engine.CreateScope();
                _engine.Execute(_gameCode, _scope);
                _game = _engine.Operations.CreateInstance(_scope.GetVariable("Game"), false);
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        public bool ValidateAnswer()
        {
            try
            {
                _game.answer("test", "one");
                return true;
            }
            catch
            {
                ErrorMessage = "Error getting answer\n";
                return false;
            }
        }

        public bool ValidateTurn()
        {
            try
            {
                _game.turn();
                return true;
            }
            catch
            {
                ErrorMessage = "Error ending turn\n";
                return false;
            }
        }

        public bool ValidateGameOver()
        {
            try
            {
                _game.gameOver();
                return true;
            }
            catch
            {
                ErrorMessage = "Error checking game over\n";
                return false;
            }
        }

        public bool ValidateWinner()
        {
            try
            {
                _game.winner();
                return true;
            }
            catch
            {
                ErrorMessage = "Error getting winner\n";
                return false;
            }
        }

        public bool ValidateGetHealth()
        {
            try
            {
                _game.getHealth("one");
                return true;
            }
            catch
            {
                ErrorMessage = "Error getting health\n";
                return false;
            }
        }
    }
}
