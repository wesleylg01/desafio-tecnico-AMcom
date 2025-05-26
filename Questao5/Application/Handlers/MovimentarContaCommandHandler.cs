using MediatR;
using Microsoft.OpenApi.Extensions;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Exceptions;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaCommandHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
    {
        private readonly IContaCorrenteCommandStore _commandStore;
        private readonly IContaCorrenteQueryStore _queryStore;

        public MovimentarContaCommandHandler(
            IContaCorrenteCommandStore commandStore,
            IContaCorrenteQueryStore queryStore)
        {
            _commandStore = commandStore;
            _queryStore = queryStore;
        }

        public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            // 1. Valida conta cadastrada
            var conta = await _queryStore.ObterContaPorIdAsync(request.IdContaCorrente);
            if (conta == null)
                throw new BusinessException(BusinessErrorType.INVALID_ACCOUNT);

            // 2. Valida conta ativa
            if (!conta.Ativo)
                throw new BusinessException(BusinessErrorType.INACTIVE_ACCOUNT);

            // 3. Valida valor positivo
            if (request.Valor <= 0)
                throw new BusinessException(BusinessErrorType.INVALID_VALUE);

            // 4. Valida tipo de movimento
            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                throw new BusinessException(BusinessErrorType.INVALID_TYPE);

            // 5. Valida idempotência
            var existe = await _commandStore.ExisteIdempotenciaAsync(request.ChaveIdempotencia);

            if (existe)
            {
                return new MovimentarContaResponse
                {
                    Mensagem = "Movimentação já realizada."
                };
            }

            // 6. Insere movimento
            var idMovimento = await _commandStore.InserirMovimentoAsync(
                request.IdContaCorrente,
                request.TipoMovimento,
                request.Valor);

            // 7. Registra idempotência
            await _commandStore.RegistrarIdempotenciaAsync(request.ChaveIdempotencia, idMovimento);

            // 8. Retorna resposta
            return new MovimentarContaResponse
            {
                IdMovimento = idMovimento
            };
        }
    }
}