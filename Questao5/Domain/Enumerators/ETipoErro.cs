using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum ETipoErro
    {
        [Description("INVALID_ACCOUNT")]
        INVALID_ACCOUNT = 1,

        [Description("INACTIVE_ACCOUNT")]
        INACTIVE_ACCOUNT = 2,

        [Description("INVALID_VALUE")]
        INVALID_VALUE = 3,

        [Description("INVALID_TYPE")]
        INVALID_TYPE = 4,
    }
}
