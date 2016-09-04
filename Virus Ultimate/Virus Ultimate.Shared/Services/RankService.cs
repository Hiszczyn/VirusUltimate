using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Virus_Ultimate.Data;

namespace Virus_Ultimate.Services
{
    class RankService
    {
        DBConnect _dbCon;
        public RankService()
        {
            _dbCon = new DBConnect();
        }

        private async void updateScores()
        {
            await _dbCon.getData();
        }

        public List<Score> getTopScores(int typeOfScore)
        {
            
            var scores = _dbCon._results.Where(r => r.Type==typeOfScore).ToList();
            if (typeOfScore == 1)
                return scores.OrderBy(s => s.Result).ToList();
            else
                return scores.OrderByDescending(s => s.Result).ToList(); 
        }

        public void addNewScore(string playerName, int result, int type)
        {
            _dbCon.addResult(playerName, result, type);
        }
    }
}
