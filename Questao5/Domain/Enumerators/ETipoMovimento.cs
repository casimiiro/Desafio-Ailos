using System.ComponentModel;

namespace Questao5.Domain.Enumerators
{
    public enum ETipoMovimento
    {
        [Description("C")]
        Credito = 'C',

        [Description("D")]
        Debito = 'D'
    }
}
