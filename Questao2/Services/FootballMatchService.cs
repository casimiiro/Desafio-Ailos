using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Questao2.Models;
using Questao2.Services.Interfaces;

namespace Questao2.Services
{
    public class FootballMatchService : IFootballMatchService
    {
        private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

        public async Task<IEnumerable<Match>> GetMatchesAsync(int year, string team, string teamRole)
        {
            List<Match> matches = new();
            int page = 1;
            bool hasMore = true;

            using var client = new HttpClient();

            while (hasMore)
            {
                var url = $"{BaseUrl}?year={year}&{teamRole}={Uri.EscapeDataString(team)}&page={page}";
                var response = await client.GetStringAsync(url);
                var json = JObject.Parse(response);
                var data = json["data"];
                int totalPages = json.Value<int>("total_pages");

                foreach (var item in data!)
                {
                    matches.Add(new Match
                    {
                        Team1 = item["team1"]!.ToString(),
                        Team2 = item["team2"]!.ToString(),
                        Team1Goals = int.Parse(item["team1goals"]!.ToString()),
                        Team2Goals = int.Parse(item["team2goals"]!.ToString())
                    });
                }

                page++;
                hasMore = page <= totalPages;
            }

            return matches;
        }
    }
}
