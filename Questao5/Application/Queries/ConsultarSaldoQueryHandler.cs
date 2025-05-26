using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Queries
{
    public class ConsultarSaldoQueryHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>
    {
        private readonly IContaCorrenteQueryStore _queryStore;

        public ConsultarSaldoQueryHandler(IContaCorrenteQueryStore queryStore)
        {
            _queryStore = queryStore;
        }

        public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoQuery request, CancellationToken cancellationToken)
        {
            var conta = await _queryStore.ObterContaPorIdAsync(request.IdContaCorrente);

            if (conta == null)
                throw new BusinessException(BusinessErrorType.INVALID_ACCOUNT);

            if (!conta.Ativo)
                throw new BusinessException(BusinessErrorType.INACTIVE_ACCOUNT);

            var saldo = await _queryStore.ObterSaldoPorContaAsync(request.IdContaCorrente);

            return new ConsultarSaldoResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Titular,
                DataConsulta = DateTime.UtcNow,
                Saldo = saldo
            };
        }
    }
}
