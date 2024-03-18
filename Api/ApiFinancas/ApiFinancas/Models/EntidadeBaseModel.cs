using System.Text.Json.Serialization;

namespace ApiFinancas.Models
{

    /// <summary>
    /// Entidade base do banco de dados do sistema
    /// </summary>
    public class EntidadeBaseModel
    {
        /// <summary>
        /// Representa um investimento.
        /// </summary>
        /// <summary>
        /// Obtém ou define o identificador único do investimento.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Obtém ou define o identificador único do usuário relacionado a este investimento.
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Obtém ou define a data de inclusão do investimento.
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Obtém ou define a data de alteração do investimento.
        /// </summary>
        public DateTime DataAlteracao { get; set; }

        /// <summary>
        /// Obtém ou define a versão do investimento.
        /// </summary>
        public int Versao { get; set; }

        /// <summary>
        /// Obtém ou define se o investimento foi excluído (true) ou não (false).
        /// </summary>
        [JsonIgnore]
        public bool VoExcluido { get; set; }
    }
}
