using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validators.Interfaces;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.tests.Saldo
{
    public class ConsultarSaldoHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentoRepository _movimentoRepository;
        private readonly IConsultaSaldoValidator _validator;
        private readonly ConsultarSaldoHandler _handler;

        public ConsultarSaldoHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentoRepository = Substitute.For<IMovimentoRepository>();
            _validator = Substitute.For<IConsultaSaldoValidator>();

            _handler = new ConsultarSaldoHandler(_contaCorrenteRepository, _validator, _movimentoRepository);
        }

        [Fact]
        public async Task Deve_Retornar_Saldo_Quando_Conta_Valida()
        {
            // Arrange
            var command = new ConsultarSaldoQuery(123);
            var conta = new ContaCorrente { Numero = 123, Nome = "Conta Teste" };
            var saldo = 1000m;

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(conta);
            _validator.Validar(command, conta).Returns(ConsultarSaldoResponse.Ok());
            _movimentoRepository.ObterSaldoAsync(123).Returns(saldo);

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal(123, resultado.NumeroConta);
            Assert.Equal("Conta Teste", resultado.NomeTitular);
            Assert.Equal("R$ " + saldo, resultado.Saldo);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Nao_Encontrada()
        {
            // Arrange
            var command = new ConsultarSaldoQuery(123);

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns((ContaCorrente)null);
            _validator.Validar(command, null).Returns(ConsultarSaldoResponse.Erro("Apenas contas correntes cadastradas podem consultar o saldo", ETipoErro.INVALID_ACCOUNT));

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INVALID_ACCOUNT.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas contas correntes cadastradas podem consultar o saldo", resultado.Mensagem);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Inativa()
        {
            // Arrange
            var command = new ConsultarSaldoQuery(123);
            var contaInativa = new ContaCorrente { Numero = 123, Nome = "Conta Teste", Ativo = 0 };
            var saldo = 1000m;

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(contaInativa);
            _validator.Validar(command, contaInativa).Returns(ConsultarSaldoResponse.Erro("Apenas contas correntes ativas podem consultar o saldo", ETipoErro.INACTIVE_ACCOUNT));
            _movimentoRepository.ObterSaldoAsync(123).Returns(saldo);

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INACTIVE_ACCOUNT.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas contas correntes ativas podem consultar o saldo", resultado.Mensagem);
        }
    }
}
