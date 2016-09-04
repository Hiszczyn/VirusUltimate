using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Virus_Ultimate.Data;

namespace Virus_Ultimate
{
    class DBConnect
    {
        public List<Score> _results;
        public async System.Threading.Tasks.Task<string> getData()
        {
            Score score;
            _results = new List<Score>();
            HttpClient http = new System.Net.Http.HttpClient();
            HttpResponseMessage response = await http.GetAsync("http://hiszczyn.cba.pl/highScore.php");
            var webresponse = await response.Content.ReadAsStringAsync();
            foreach( var a in webresponse.Split(new string[] { "@mySeparator@" }, StringSplitOptions.None)){
                score = new Score();
                if(a.Split(':').Length==3)
                {
                    score.PlayerName = a.Split(':')[0];
                    score.Result = Convert.ToInt16(a.Split(':')[1]);
                    score.Type = Convert.ToInt16(a.Split(':')[2]);
                    _results.Add(score);
                }
            }
            return "success";
        }
        public async void addResult(string playerName, int result, int type)
        {
            _results = new List<Score>();
            HttpClient http = new System.Net.Http.HttpClient();
            string url = ("http://hiszczyn.cba.pl/AddRecord.php?name='" + playerName + "'&rcrd=" + result + "&type=" + type);
            HttpResponseMessage response = await http.GetAsync(url);
            var webresponse = await response.Content.ReadAsStringAsync();
            //getData();
        }


    }
}
