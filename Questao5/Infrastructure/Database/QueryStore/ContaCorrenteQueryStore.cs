using Dapper;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore
{
    public class ContaCorrenteQueryStore : IContaCorrenteQueryStore
    {
        private readonly IDbConnection _dbConnection;

        public ContaCorrenteQueryStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ContaCorrente> ObterContaPorIdAsync(string idContaCorrente)
        {
            var sql = @"SELECT idcontacorrente AS IdContaCorrente, 
                               numero AS Numero, 
                               nome AS Titular, 
                               CASE WHEN ativo = 1 THEN 1 ELSE 0 END AS Ativo
                        FROM contacorrente 
                        WHERE idcontacorrente = @IdContaCorrente";

            return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { IdContaCorrente = idContaCorrente });
        }

        public async Task<decimal> ObterSaldoPorContaAsync(string idContaCorrente)
        {
            var sqlCreditos = @"SELECT IFNULL(SUM(valor), 0) 
                                FROM movimento 
                                WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'C'";

            var sqlDebitos = @"SELECT IFNULL(SUM(valor), 0) 
                               FROM movimento 
                               WHERE idcontacorrente = @IdContaCorrente AND tipomovimento = 'D'";

            var creditos = await _dbConnection.ExecuteScalarAsync<decimal>(sqlCreditos, new { IdContaCorrente = idContaCorrente });
            var debitos = await _dbConnection.ExecuteScalarAsync<decimal>(sqlDebitos, new { IdContaCorrente = idContaCorrente });

            return creditos - debitos;
        }
    }
}
