using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries;

namespace Questao5.Infrastructure.Services.Controllers
{
    /// <summary>
    /// Controller responsável por operações relacionadas à conta corrente.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Consulta o saldo da conta corrente.
        /// </summary>
        /// <param name="id">Identificador da conta corrente.</param>
        /// <returns>Dados do saldo da conta corrente.</returns>
        /// <response code="200">Saldo consultado com sucesso.</response>
        /// <response code="400">Conta inválida ou inativa.</response>
        /// <response code="500">Erro interno do servidor.</response>
        [HttpGet("{id}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string id)
        {
            try
            {
                var query = new ConsultarSaldoQuery { IdContaCorrente = id };
                var response = await _mediator.Send(query);
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
