namespace Questao5.Domain.Entities
{
    public class Movimento
    {
        public string Id { get; }
        public string IdContaCorrente { get; }
        public DateTime DataMovimento { get; }
        public string TipoMovimento { get; }
        public decimal Valor { get; }

        public Movimento(string id, string idContaCorrente, DateTime dataMovimento, string tipo, decimal valor)
        {
            Id = id;
            IdContaCorrente = idContaCorrente;
            DataMovimento = dataMovimento;
            TipoMovimento = tipo;
            Valor = valor;
        }
    }

}
