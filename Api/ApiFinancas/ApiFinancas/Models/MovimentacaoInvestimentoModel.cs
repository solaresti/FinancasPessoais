namespace ApiFinancas.Models
{
    /// <summary>
    /// Model extrato de uma determinada meta
    /// </summary>
    public class MovimentacaoInvestimentoModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o valor de uma movimentação de investimento.
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Data da movimentação de investimento.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Obtém e define o Id da Meta relativa à movimentação do investimento.
        /// </summary>
        public int IdMeta { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        public int IdInvestimento { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        public string Descritivo { get; set; }

    }
    
}