using Dapper;
using System;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore
{
    public class ContaCorrenteCommandStore : IContaCorrenteCommandStore
    {
        private readonly IDbConnection _connection;

        public ContaCorrenteCommandStore(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<string> InserirMovimentoAsync(string idConta, string tipo, decimal valor)
        {
            var idMovimento = Guid.NewGuid().ToString();

            var sql = @"
                INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) 
                VALUES (@Id, @Conta, @Data, @Tipo, @Valor);
            ";

            await _connection.ExecuteAsync(sql, new
            {
                Id = idMovimento,
                Conta = idConta,
                Data = DateTime.UtcNow,
                Tipo = tipo,
                Valor = valor
            });

            return idMovimento;
        }

        public async Task<bool> ExisteIdempotenciaAsync(string chave)
        {
            var sql = @"SELECT COUNT(*) FROM idempotencia WHERE chave_idempotencia = @Chave";

            var count = await _connection.ExecuteScalarAsync<int>(sql, new { Chave = chave });

            return count > 0;
        }

        public async Task RegistrarIdempotenciaAsync(string chave, string resultado)
        {
            var sql = @"
                INSERT INTO idempotencia (chave_idempotencia, resultado) 
                VALUES (@Chave, @Resultado);
            ";

            await _connection.ExecuteAsync(sql, new
            {
                Chave = chave,
                Resultado = resultado
            });
        }
    }
}
