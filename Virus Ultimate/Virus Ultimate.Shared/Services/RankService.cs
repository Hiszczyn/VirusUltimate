using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<Score>> getTopScores()
        {
            var dirtyResults = await _dbCon.getData();
            var cleanedResult = CleanResults(dirtyResults);
            return cleanedResult;
            
        }

        public Task<string> addNewScore(string playerName, int result, int type)
        {
            return _dbCon.addResult(playerName, result, type);
        }

        public List<Score> ScoreByTypeFilter(List<Score> scores, int type)
        {
            var filteredScores =  scores.Where(r => r.Type == type).ToList();
            if (type == 1)
                return filteredScores.OrderBy(s => s.Result).ToList();
            else
                return filteredScores.OrderByDescending(s => s.Result).ToList();
        }

        private List<Score> CleanResults(string webresponse)
        {
            var cleanedResults = new List<Score>();
            foreach (var a in webresponse.Split(new string[] { "@mySeparator@" }, StringSplitOptions.None))
            {
                Score score;
                score = new Score();
                if (a.Split(':').Length == 3)
                {
                    score.PlayerName = a.Split(':')[0];
                    score.Result = Convert.ToInt16(a.Split(':')[1]);
                    score.Type = Convert.ToInt16(a.Split(':')[2]);
                    cleanedResults.Add(score);
                }
            }
            return cleanedResults;
        }
    }
}
