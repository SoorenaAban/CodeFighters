using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace CodeFighters.GameMaster
{
    public class GameCodeHost
    {
        private readonly string _gameCode;
        private ScriptEngine _engine;
        private ScriptScope _scope;
        private dynamic _game;

        public GameCodeHost(string gameCode)
        {
            _gameCode = gameCode;
        }

        public bool IsGameCodeValid(string gameCode)
        {
            try
            {
                var engine = Python.CreateEngine();
                var scope = engine.CreateScope();
                engine.Execute(gameCode, scope);
                var game = engine.Operations.CreateInstance(scope.GetVariable("Game"));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StartGame(string gameCode, bool isVsAI)
        {
            if (!IsGameCodeValid(gameCode))
                return false;


            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();
            _engine.Execute(gameCode, _scope);
            _game = _engine.Operations.CreateInstance(_scope.GetVariable("Game"), isVsAI);
            return _game.start();
        }

        public bool SubmitAnswer(string answer, string player)
        {
            return _game.answer(answer, player);
        }

        public string SubmitTurnEnd()
        {
            return _game.turn();
        }

        public bool IsGameOver()
        {
            return _game.gameOver();
        }

        public int GetWinner()
        {
            return _game.winner();
        }

        public string GetValue(string value, string player)
        {
            return _game.getHealth(player);
        }

        public string GetGameValue(string value)
        {
            throw new NotImplementedException();
        }
    }
}
