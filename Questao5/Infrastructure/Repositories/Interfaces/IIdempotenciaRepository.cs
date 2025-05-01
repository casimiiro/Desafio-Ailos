using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;

namespace Questao5.Infrastructure.Repositories.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task<ResultadoMovimentacao?> ObterResultadoAsync(string chaveIdempotencia);
        Task<ResultadoMovimentacao?> SalvarResultadoAsync(string chaveIdempotencia, ResultadoMovimentacao resultado, MovimentarContaCommand requisicao);
    }
}
