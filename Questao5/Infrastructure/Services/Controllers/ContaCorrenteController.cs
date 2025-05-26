using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Queries;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContaCorrenteController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
