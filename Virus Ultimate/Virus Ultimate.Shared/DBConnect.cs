using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Virus_Ultimate.Data;

namespace Virus_Ultimate
{
    class DBConnect
    {
        private static string _URL = "http://hiszczyn.cba.pl/";
        public async Task<string> getData()
        {
            HttpClient http = new HttpClient();
            HttpResponseMessage response = await http.GetAsync(_URL + "highScore.php");
            return await response.Content.ReadAsStringAsync();
        }
        public async Task<string> addResult(string playerName, int result, int type)
        {
            HttpClient http = new HttpClient();
            string url = (_URL + "AddRecord.php?name='" + playerName + "'&rcrd=" + result + "&type=" + type);
            HttpResponseMessage response = await http.GetAsync(url);
            var webresponse = await response.Content.ReadAsStringAsync();
            return webresponse;
        }


    }
}
