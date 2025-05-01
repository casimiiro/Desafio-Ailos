using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Factories.Interfaces;
using Questao5.Domain.Validators.Interfaces;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, ResultadoMovimentacao>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IMovimentacaoValidator _validator;
        private readonly IMovimentoFactory _movimentoFactory;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentarContaHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentoRepository movimentoRepository,
            IMovimentacaoValidator validator,
            IMovimentoFactory movimentoFactory,
            IIdempotenciaRepository idempotenciaRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentoRepository = movimentoRepository;
            _validator = validator;
            _movimentoFactory = movimentoFactory;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<ResultadoMovimentacao> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {

            var resultadoExistente = await _idempotenciaRepository.ObterResultadoAsync(request.IdRequisicao);
            if (resultadoExistente != null)
                return resultadoExistente;

            var conta = await _contaCorrenteRepository.ObterPorIdAsync(request.NumeroConta);
            var resultadoValidacao = _validator.Validar(request, conta);
            if (!resultadoValidacao.Sucesso)
            {
                await _idempotenciaRepository.SalvarResultadoAsync(request.IdRequisicao, resultadoValidacao, request);
                return resultadoValidacao;
            }

            var movimento = _movimentoFactory.Criar(conta.IdContaCorrente, request.TipoMovimento, request.Valor);

            await _movimentoRepository.RegistrarMovimentoAsync(movimento);

            var resultado = ResultadoMovimentacao.Ok(movimento.Id);
            await _idempotenciaRepository.SalvarResultadoAsync(request.IdRequisicao, resultado, request);

            return resultado;
        }
    }
}
