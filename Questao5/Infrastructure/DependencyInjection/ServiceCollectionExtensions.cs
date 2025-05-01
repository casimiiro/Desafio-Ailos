using Questao5.Domain.Factories;
using Questao5.Domain.Factories.Interfaces;
using Questao5.Domain.Validators.Interfaces;
using Questao5.Domain.Validators.Movimento;
using Questao5.Domain.Validators.Saldo;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            #region Repository
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IMovimentoRepository, MovimentoRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
            #endregion

            #region Validator
            services.AddScoped<IMovimentacaoValidator, MovimentacaoValidator>();
            services.AddScoped<IConsultaSaldoValidator, ConsultaSaldoValidator>();
            #endregion

            #region Factory
            services.AddScoped<IMovimentoFactory, MovimentoFactory>();
            #endregion

            return services;
        }
    }
}
