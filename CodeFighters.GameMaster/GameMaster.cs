using CodeFighters.Data;
using CodeFighters.Models;
using System.Threading;

namespace CodeFighters.GameMaster
{
    public class GameMaster : IGameMaster
    {
        public List<GameWorker> ActiveGames { get; private set; }
        public List<Thread> ActiveGameThreads { get; private set; }

        public string GameCode { get; set; }

        public GameMaster()
        {
            ActiveGames = new List<GameWorker>();
            ActiveGameThreads = new List<Thread>();
        }

        public GameWorker CreateGame(GameModel gameModel, ApiContext apiContext)
        {
            var gameHost = new GameCodeHost(GameCode);
            var game = new GameWorker(gameModel, apiContext, gameHost);
            ActiveGames.Add(game);
            return game;
        }

        public void StartGame(Guid gameId)
        {
            var gameWorker = ActiveGames.FirstOrDefault(g => g.Game.Id == gameId);

            if (gameWorker == null)
            {
                throw new ArgumentException("GameWorker not found");
            }

            var thread = new Thread(new ThreadStart(gameWorker.Run));
            ActiveGameThreads.Add(thread);

            thread.Start();
        }

        public GameWorker? GetGameWorker(Guid gameId)
        {
            if (ActiveGames == null)
            {
                return null;
            }

            var temp =  ActiveGames.FirstOrDefault(g => g.Game.Id == gameId);

            return temp;
        }
    }
}
