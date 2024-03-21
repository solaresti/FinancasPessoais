namespace ApiFinancas.Models
{
    public class DespesaModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o id da categoria da despesa.
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Obtém ou define o valor da despesa.
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da despesa.
        /// </summary>
        public string Descritivo { get; set; }

        /// <summary>
        /// Data da despesa
        /// </summary>
        public DateTime Data { get; set; }
    }
}
