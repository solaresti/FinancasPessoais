namespace ApiFinancas.Models
{
    public class CategoriaDespesaModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o nome da categoria de despesa.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define o tipo da categoria de despesa.
        /// </summary>
        public int TipoDespesa { get; set; }

        /// <summary>
        /// Obtém ou define o valor da meta mensal da categoria da despesa.
        /// </summary>
        public float MetaValorMensal { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da categoria da despesa.
        /// </summary>
        public string Descritivo { get; set; }
    }
}
