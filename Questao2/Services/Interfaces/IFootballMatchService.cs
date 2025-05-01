using Questao2.Models;

namespace Questao2.Services.Interfaces
{
    public interface IFootballMatchService
    {
        Task<IEnumerable<Match>> GetMatchesAsync(int year, string team, string teamRole);
    }
}
