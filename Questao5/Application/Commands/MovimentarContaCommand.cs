using MediatR;
using Questao5.Application.Commands.Responses;

namespace Questao5.Application.Commands
{
    public class MovimentarContaCommand : IRequest<MovimentarContaResponse>
    {
        public string IdRequisicao { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
        public string ChaveIdempotencia { get; set; }
    }
}
