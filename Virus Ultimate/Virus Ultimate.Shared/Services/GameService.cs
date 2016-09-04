using System;
using System.Collections.Generic;
using System.Text;

namespace Virus_Ultimate.Services
{
    class GameService
    {
        public CurrentGame _game;

        public GameService()
        {
            _game = new CurrentGame();
            cleanGame();
        }

        public void cleanGame()
        {
            _game.BestMove = 0;
            _game.BestTime = 99999;
            _game.Score = 0;
        }

        public void updateScore(int infected,int move, int time)
        {
            if (time == -1)
            {
                _game.Score = _game.Score + infected;
                if (move > _game.BestMove)
                    _game.BestMove = move;
            }
            else
            {
                _game.Score = _game.Score + 400 + move;
                if (time < _game.BestTime)
                    _game.BestTime = time;
                if (move > _game.BestMove)
                    _game.BestMove = move;
            }
            
        }
    }
}
