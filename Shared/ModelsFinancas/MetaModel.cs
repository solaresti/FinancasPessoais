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
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        [Required(ErrorMessage ="A descrição é obrigatória")]
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