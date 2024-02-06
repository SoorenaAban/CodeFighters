using CodeFighters.Data;
using CodeFighters.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFighters.GameMaster
{
    public interface IGameMaster
    {
        GameWorker CreateGame(GameModel gameModel, ApiContext apiContext);
        void StartGame(Guid gameId);

        GameWorker? GetGameWorker(Guid gameId);

        string GameCode { get;  set; }
    }
}
