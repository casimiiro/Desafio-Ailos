using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Validators.Interfaces;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Application.Handlers
{
    public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IConsultaSaldoValidator _validator;
        private readonly IMovimentoRepository _movimentoRepository;

        public ConsultarSaldoHandler(IContaCorrenteRepository contaCorrenteRepository, IConsultaSaldoValidator validator, IMovimentoRepository movimentoRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _validator = validator;
            _movimentoRepository = movimentoRepository;
        }

        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.ObterPorIdAsync(request.NumeroConta);

            var resultadoValidacao = _validator.Validar(request, contaCorrente);
            if (!resultadoValidacao.Sucesso)
                return resultadoValidacao;

            var saldo = await _movimentoRepository.ObterSaldoAsync(request.NumeroConta);

            return ConsultarSaldoResponse.Ok(
                contaCorrente.Numero,
                contaCorrente.Nome,
                DateTime.UtcNow,
                saldo
            );
        }
    }
}
