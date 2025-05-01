using Questao2.Services.Interfaces;

namespace Questao2.Business
{
    public class GoalCalculator
    {
        private readonly IFootballMatchService _matchService;

        public GoalCalculator(IFootballMatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task<int> GetTotalGoalsAsync(string team, int year)
        {
            var team1Matches = await _matchService.GetMatchesAsync(year, team, "team1");
            var team2Matches = await _matchService.GetMatchesAsync(year, team, "team2");

            int goalsAsTeam1 = team1Matches.Sum(m => m.Team1Goals);
            int goalsAsTeam2 = team2Matches.Sum(m => m.Team2Goals);

            return goalsAsTeam1 + goalsAsTeam2;
        }
    }
}
