using FluentAssertions;
using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

namespace Questao5.Tests.Application.Commands
{
    public class MovimentarContaCommandHandlerTests
    {
        private readonly IContaCorrenteCommandStore _commandStoreSubstitute;
        private readonly IContaCorrenteQueryStore _queryStoreSubstitute;
        private readonly MovimentarContaCommandHandler _handler;

        public MovimentarContaCommandHandlerTests()
        {
            _commandStoreSubstitute = Substitute.For<IContaCorrenteCommandStore>();
            _queryStoreSubstitute = Substitute.For<IContaCorrenteQueryStore>();
            _handler = new MovimentarContaCommandHandler(_commandStoreSubstitute, _queryStoreSubstitute);
        }

        [Fact]
        public async Task Handle_InserirMovimentoERegistrarIdempotencia_Sucesso()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", Valor = 50.75m, TipoMovimento = "D", ChaveIdempotencia = "6cc94d70-9a83-4bbe-bacc-ff9ffa9a1210" };
            _queryStoreSubstitute.ObterContaPorIdAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44").Returns(Task.FromResult(new ContaCorrente { Ativo = true }));
            _commandStoreSubstitute.ExisteIdempotenciaAsync("6cc94d70-9a83-4bbe-bacc-ff9ffa9a1210").Returns(Task.FromResult(false));
            _commandStoreSubstitute.InserirMovimentoAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", "D", 50.75m).Returns(Task.FromResult("1c9a0d42-5a2b-4059-ad09-4cf5975cd08e"));

            // Act
            var response = await _handler.Handle(request, default);

            // Assert
            response.Should().NotBeNull();
            response.IdMovimento.Should().Be("1c9a0d42-5a2b-4059-ad09-4cf5975cd08e");
            await _commandStoreSubstitute.Received(1).InserirMovimentoAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", "D", 50.75m);
            await _commandStoreSubstitute.Received(1).RegistrarIdempotenciaAsync("6cc94d70-9a83-4bbe-bacc-ff9ffa9a1210", "1c9a0d42-5a2b-4059-ad09-4cf5975cd08e");
        }

        [Fact]
        public async Task Handle_ContaNaoExistente_Erro()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "contaInexistente", Valor = 10, TipoMovimento = "C", ChaveIdempotencia = "chave" };
            _queryStoreSubstitute.ObterContaPorIdAsync("contaInexistente").Returns(Task.FromResult<ContaCorrente>(null));

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .Where(e => e.BusinessErrorType == BusinessErrorType.INVALID_ACCOUNT);
        }

        [Fact]
        public async Task Handle_ContaInativa_Erro()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "contaInativa", Valor = 10, TipoMovimento = "C", ChaveIdempotencia = "chave" };
            _queryStoreSubstitute.ObterContaPorIdAsync("contaInativa").Returns(Task.FromResult(new ContaCorrente { Ativo = false }));

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .Where(e => e.BusinessErrorType == BusinessErrorType.INACTIVE_ACCOUNT);
        }

        [Fact]
        public async Task Handle_ValorZeradoOuNegativo_Erro()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", Valor = 0, TipoMovimento = "C", ChaveIdempotencia = "chave" };
            _queryStoreSubstitute.ObterContaPorIdAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44").Returns(Task.FromResult(new ContaCorrente { Ativo = true }));

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .Where(e => e.BusinessErrorType == BusinessErrorType.INVALID_VALUE);
        }

        [Fact]
        public async Task Handle_TipoMovimentoInvalido_Erro()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", Valor = 10, TipoMovimento = "X", ChaveIdempotencia = "chave" };
            _queryStoreSubstitute.ObterContaPorIdAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44").Returns(Task.FromResult(new ContaCorrente { Ativo = true }));

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .Where(e => e.BusinessErrorType == BusinessErrorType.INVALID_TYPE);
        }

        [Fact]
        public async Task Handle_MovimentacaoJaRealizada()
        {
            // Arrange
            var request = new MovimentarContaCommand { IdContaCorrente = "b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44", Valor = 10, TipoMovimento = "C", ChaveIdempotencia = "chaveExistente" };
            _queryStoreSubstitute.ObterContaPorIdAsync("b20fc2c3-0fa2-4f2c-a929-06adf1b2ff44").Returns(Task.FromResult(new ContaCorrente { Ativo = true }));
            _commandStoreSubstitute.ExisteIdempotenciaAsync("chaveExistente").Returns(Task.FromResult(true));

            // Act
            var response = await _handler.Handle(request, default);

            // Assert
            response.Should().NotBeNull();
            response.Mensagem.Should().Be("Movimentação já realizada.");
            await _commandStoreSubstitute.DidNotReceive().InserirMovimentoAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>());
            await _commandStoreSubstitute.DidNotReceive().RegistrarIdempotenciaAsync(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
