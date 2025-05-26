using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum BusinessErrorType
    {
        [Description("Apenas contas correntes cadastradas podem receber movimentação")]
        INVALID_ACCOUNT,

        [Description("Apenas contas correntes ativas podem receber movimentação")]
        INACTIVE_ACCOUNT,

        [Description("Apenas valores positivos podem ser recebidos")]
        INVALID_VALUE,

        [Description("Apenas os tipos “débito” ou “crédito” podem ser aceitos")]
        INVALID_TYPE
    }
}
