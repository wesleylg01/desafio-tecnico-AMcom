using FluentAssertions;
using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

namespace Questao5.Application.Queries
{
    public class ConsultarSaldoQueryHandlerTests
    {
        private readonly IContaCorrenteQueryStore _queryStoreSubstitute;
        private readonly ConsultarSaldoQueryHandler _handler;

        public ConsultarSaldoQueryHandlerTests()
        {
            _queryStoreSubstitute = Substitute.For<IContaCorrenteQueryStore>();
            _handler = new ConsultarSaldoQueryHandler(_queryStoreSubstitute);
        }

        [Fact]
        public async Task Handle_ContaExistenteEAtiva_Sucesso()
        {
            // Arrange
            var request = new ConsultarSaldoQuery { IdContaCorrente = "B6BAFC09 -6967-ED11-A567-055DFA4A16C9" };

            _queryStoreSubstitute.ObterContaPorIdAsync("B6BAFC09 -6967-ED11-A567-055DFA4A16C9").Returns(new ContaCorrente
            {
                IdContaCorrente = "B6BAFC09 -6967-ED11-A567-055DFA4A16C9",
                Numero = "001",
                Titular = "Katherine Sanchez",
                Ativo = true
            });

            _queryStoreSubstitute.ObterSaldoPorContaAsync("B6BAFC09 -6967-ED11-A567-055DFA4A16C9").Returns(150.50m);

            // Act
            var response = await _handler.Handle(request, default);

            // Assert
            response.Should().NotBeNull();
            response.NumeroConta.Should().Be("001");
            response.NomeTitular.Should().Be("Katherine Sanchez");
            response.Saldo.Should().Be(150.50m);
            response.DataConsulta.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public async Task Handle_ContaNaoExistente_Erro()
        {
            // Arrange
            var request = new ConsultarSaldoQuery { IdContaCorrente = "XXBAFC09 -6967-ED11-A567-055DFA4A16C9" };

            _queryStoreSubstitute.ObterContaPorIdAsync("XXBAFC09 -6967-ED11-A567-055DFA4A16C9")
                .Returns((ContaCorrente)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"{BusinessErrorType.INVALID_ACCOUNT}*");
        }

        [Fact]
        public async Task Handle_ContaInativa_Erro()
        {
            // Arrange
            var request = new ConsultarSaldoQuery { IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9" };

            _queryStoreSubstitute.ObterContaPorIdAsync("F475F943-7067-ED11-A06B-7E5DFA4A16C9").Returns(new ContaCorrente
            {
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
                Numero = "741",
                Titular = "Ameena Lynn",
                Ativo = false
            });

            // Act
            Func<Task> act = async () => await _handler.Handle(request, default);

            // Assert
            await act.Should().ThrowAsync<BusinessException>()
                .WithMessage($"{BusinessErrorType.INACTIVE_ACCOUNT}*");
        }
    }
}