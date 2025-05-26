using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public interface IContaCorrenteQueryStore
    {
        Task<ContaCorrente> ObterContaPorIdAsync(string idContaCorrente);

        Task<decimal> ObterSaldoPorContaAsync(string idContaCorrente);
    }
}
