namespace Questao5.Infrastructure.Database.CommandStore
{
    public interface IContaCorrenteCommandStore
    {
        Task<string> InserirMovimentoAsync(string idConta, string tipo, decimal valor);
        Task<bool> ExisteIdempotenciaAsync(string chave);
        Task RegistrarIdempotenciaAsync(string chave, string resultado);
    }
}
