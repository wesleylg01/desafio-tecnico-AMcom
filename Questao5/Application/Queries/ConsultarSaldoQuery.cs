using Questao5.Application.Queries.Responses;
using MediatR;

namespace Questao5.Application.Queries
{
    public class ConsultarSaldoQuery : IRequest<ConsultarSaldoResponse>
    {
        public string IdContaCorrente { get; set; }
    }
}