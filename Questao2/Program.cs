using Questao2.Services;
using Questao2.Business;

class Program
{
    static async Task Main()
    {
        var matchService = new FootballMatchService();
        var goalCalculator = new GoalCalculator(matchService);

        await PrintTeamGoalsAsync(goalCalculator, "Paris Saint-Germain", 2013);
        await PrintTeamGoalsAsync(goalCalculator, "Chelsea", 2014);
    }

    private static async Task PrintTeamGoalsAsync(GoalCalculator calculator, string team, int year)
    {
        int totalGoals = await calculator.GetTotalGoalsAsync(team, year);
        Console.WriteLine($"Team {team} scored {totalGoals} goals in {year}");
    }
}