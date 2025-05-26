using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;

namespace Questao5.Infrastructure.Services.Controllers
{
    /// <summary>
    /// Controller responsável por operações de movimentação de conta corrente.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovimentacaoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        /// <summary>
        /// Realiza uma movimentação na conta corrente.
        /// </summary>
        /// <param name="command">Dados da movimentação: id da requisição, id da conta, valor, tipo e chave de idempotência.</param>
        /// <returns>Identificador da movimentação realizada.</returns>
        /// <response code="200">Movimentação realizada com sucesso.</response>
        /// <response code="400">Erro de validação ou regra de negócio violada.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpPost]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentarContaCommand command)
        {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { erro = "Erro interno ao processar a requisição." });
            }
        }
    }
}
