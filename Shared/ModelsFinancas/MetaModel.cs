using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "O  nome da meta é obrigatório")]
        public string Nome { get; set; } = "";

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        [Required(ErrorMessage = "A descrição é obrigatória")]
        public string Descritivo { get; set; } = "";

        /// <summary>
        /// Valor da meta.
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Valor já reservado para a meta
        /// </summary>
        public float? ValorJaTenho { get; set; }

        /// <summary>
        /// Valor já reservado para a meta
        /// </summary>
        public float QuantoFalta
        {
            get
            {
                float valorJaTenho = ValorJaTenho == null ? 0: ValorJaTenho.Value;
                return Valor - valorJaTenho;
            }
        }



        /// <summary>
        /// Data da meta
        /// </summary>
        public DateTime Data { get; set; }
    }
}