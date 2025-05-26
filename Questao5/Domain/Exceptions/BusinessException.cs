using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;

namespace Questao5.Domain.Exceptions
{
    public class BusinessException : InvalidOperationException
    {
        public BusinessErrorType BusinessErrorType { get; }
        public BusinessException(BusinessErrorType errorType)
            : base($"{errorType} - {errorType.GetDescription()}")
        {
            BusinessErrorType = errorType;
        }
    }
}
