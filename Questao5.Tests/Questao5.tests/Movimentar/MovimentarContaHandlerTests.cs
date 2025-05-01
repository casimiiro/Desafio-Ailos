using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Factories.Interfaces;
using Questao5.Domain.Validators.Interfaces;
using Questao5.Infrastructure.Repositories.Interfaces;

namespace Questao5.tests.Movimentar
{
    public class MovimentarContaHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
        private readonly IMovimentoRepository _movimentoRepository = Substitute.For<IMovimentoRepository>();
        private readonly IMovimentoFactory _movimentoFactory = Substitute.For<IMovimentoFactory>();
        private readonly IMovimentacaoValidator _validator = Substitute.For<IMovimentacaoValidator>();
        private readonly IIdempotenciaRepository _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
        private readonly MovimentarContaHandler _handler;

        public MovimentarContaHandlerTests()
        {
            _handler = new MovimentarContaHandler(_contaCorrenteRepository, _movimentoRepository, _validator, _movimentoFactory, _idempotenciaRepository);
        }

        [Fact]
        public async Task Deve_Retornar_Sucesso_Quando_Conta_Valida_E_Valor_Valido()
        {
            // Arrange
            var command = new MovimentarContaCommand
            {
                NumeroConta = 123,
                TipoMovimento = ETipoMovimento.Credito.GetDescription(),
                Valor = 100
            };

            var conta = new ContaCorrente { IdContaCorrente = "123", Ativo = 1 };
            var movimento = new Movimento("abc", "123", DateTime.Now, "Credito", 100);

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(conta);
            _validator.Validar(command, conta).Returns(ResultadoMovimentacao.Ok("abc"));
            _movimentoFactory.Criar("123", ETipoMovimento.Credito.GetDescription(), 100).Returns(movimento);

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.True(resultado.Sucesso);
            Assert.Equal("abc", resultado.IdMovimento);
            await _movimentoRepository.Received().RegistrarMovimentoAsync(movimento);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Nao_Cadastrada()
        {
            // Arrange
            var command = new MovimentarContaCommand
            {
                NumeroConta = 123,
                TipoMovimento = ETipoMovimento.Credito.GetDescription(),
                Valor = 100
            };

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns((ContaCorrente?)null);

            _validator.Validar(command, null).Returns(ResultadoMovimentacao.Erro("Apenas contas correntes cadastradas podem receber movimentação", ETipoErro.INVALID_ACCOUNT));

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INVALID_ACCOUNT.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas contas correntes cadastradas podem receber movimentação", resultado.Mensagem);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Conta_Inativa()
        {
            // Arrange
            var command = new MovimentarContaCommand
            {
                NumeroConta = 123,
                TipoMovimento = ETipoMovimento.Credito.GetDescription(),
                Valor = 100
            };

            var conta = new ContaCorrente { IdContaCorrente = "123", Ativo = 0 };

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(conta);
            _validator.Validar(command, conta).Returns(ResultadoMovimentacao.Erro("Apenas contas correntes ativas podem receber movimentação", ETipoErro.INACTIVE_ACCOUNT));

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INACTIVE_ACCOUNT.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas contas correntes ativas podem receber movimentação", resultado.Mensagem);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Valor_Menor_Que_Zero()
        {
            // Arrange
            var command = new MovimentarContaCommand
            {
                NumeroConta = 123,
                TipoMovimento = ETipoMovimento.Credito.GetDescription(),
                Valor = -10
            };

            var conta = new ContaCorrente { IdContaCorrente = "123", Ativo = 1 };

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(conta);
            _validator.Validar(command, conta).Returns(ResultadoMovimentacao.Erro("Apenas valores positivos podem ser recebidos", ETipoErro.INVALID_VALUE));

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INVALID_VALUE.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas valores positivos podem ser recebidos", resultado.Mensagem);
        }

        [Fact]
        public async Task Deve_Retornar_Erro_Quando_Tipo_Movimento_Invalido()
        {
            // Arrange
            var command = new MovimentarContaCommand
            {
                NumeroConta = 123,
                TipoMovimento = "T",
                Valor = 100
            };

            var conta = new ContaCorrente { IdContaCorrente = "123", Ativo = 1 };

            _contaCorrenteRepository.ObterPorIdAsync(123).Returns(conta);
            _validator.Validar(command, conta).Returns(ResultadoMovimentacao.Erro("Apenas os tipos “débito” ou “crédito” podem ser aceitos", ETipoErro.INVALID_TYPE));

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.False(resultado.Sucesso);
            Assert.Equal(ETipoErro.INVALID_TYPE.GetDescription(), resultado.TipoErro);
            Assert.Equal("Apenas os tipos “débito” ou “crédito” podem ser aceitos", resultado.Mensagem);
        }

        [Fact]
        public async Task Deve_Retornar_ResultadoExistente_Quando_IdRequisicao_Ja_Processada()
        {
            // Arrange
            var idRequisicao = Guid.NewGuid().ToString();
            var resultadoExistente = ResultadoMovimentacao.Ok("123");

            var command = new MovimentarContaCommand
            {
                IdRequisicao = idRequisicao,
                NumeroConta = 123,
                TipoMovimento = ETipoMovimento.Credito.GetDescription(),
                Valor = 100
            };

            _idempotenciaRepository.ObterResultadoAsync(idRequisicao).Returns(resultadoExistente);

            // Act
            var resultado = await _handler.Handle(command, default);

            // Assert
            Assert.Same(resultadoExistente, resultado);
            await _contaCorrenteRepository.DidNotReceive().ObterPorIdAsync(Arg.Any<int>());
            await _movimentoRepository.DidNotReceive().RegistrarMovimentoAsync(Arg.Any<Movimento>());
            await _idempotenciaRepository.DidNotReceive().SalvarResultadoAsync(Arg.Any<string>(), Arg.Any<ResultadoMovimentacao>(), Arg.Any<MovimentarContaCommand>());
        }
    }
}
