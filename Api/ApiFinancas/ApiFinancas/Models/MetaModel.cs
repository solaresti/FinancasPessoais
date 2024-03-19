namespace ApiFinancas.Models
{
    /// <summary>
    /// Model de Metas
    /// </summary>
    public class MetaModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o nome da meta.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        public string Descritivo { get; set; }

        /// <summary>
        /// Obtém ou define o valor da meta.
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Data da meta
        /// </summary>
        public DateTime Data { get; set; }
    }
}