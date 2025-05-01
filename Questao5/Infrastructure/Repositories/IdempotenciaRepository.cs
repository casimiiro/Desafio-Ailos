using System.Data;
using System.Text.Json;
using Dapper;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Infrastructure.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly IDbConnection _dbConnection;

        public IdempotenciaRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ResultadoMovimentacao?> ObterResultadoAsync(string chaveIdempotencia)
        {
            const string sql = @"SELECT resultado
                                 FROM idempotencia
                                 WHERE chave_idempotencia = @ChaveIdempotencia";

            var resultadoJson = await _dbConnection.QueryFirstOrDefaultAsync<string>(sql, new
            {
                ChaveIdempotencia = chaveIdempotencia
            });

            if (string.IsNullOrWhiteSpace(resultadoJson))
                return null;

            return JsonSerializer.Deserialize<ResultadoMovimentacao>(resultadoJson);
        }

        public async Task<ResultadoMovimentacao?> SalvarResultadoAsync(string chaveIdempotencia, ResultadoMovimentacao resultado, MovimentarContaCommand requisicao)
        {
            const string sql = @"INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado)
                                 VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";

            var requisicaoJson = JsonSerializer.Serialize(requisicao);
            var resultadoJson = JsonSerializer.Serialize(resultado);

            try
            {
                await _dbConnection.ExecuteAsync(sql, new
                {
                    ChaveIdempotencia = chaveIdempotencia.ToString(),
                    Requisicao = requisicaoJson,
                    Resultado = resultadoJson
                });

                return resultado;
            }
            catch (Exception)
            {
                return await ObterResultadoAsync(chaveIdempotencia);
            }
        }
    }
}
