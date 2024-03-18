namespace ApiFinancas.Models
{
    public class InvestimentoModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o nome do investimento.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a data de vencimento do investimento (pode ser nulo).
        /// </summary>
        public DateTime? Vencimento { get; set; }

        /// <summary>
        /// Obtém ou define a categoria do investimento (valor inteiro).
        /// </summary>
        public int Categoria { get; set; }

        /// <summary>
        /// Obtém ou define o nível de risco do investimento (valor inteiro).
        /// </summary>
        public int Risco { get; set; }
    }
}

