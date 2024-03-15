using ApiFinancas.Enums;
using System.Text.Json.Serialization;

namespace ApiFinancas.Models

{
    /// <summary>
    /// Representa a entidade de Usuário.
    /// </summary>
    public class UsuarioModel
    {
        /// <summary>
        /// Identificador único do usuário.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Login do usuário
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Data de inclusão do usuário no sistema.
        /// </summary>
        public DateTime DataInclusao { get; set; }

        /// <summary>
        /// Data da última alteração nos dados do usuário.
        /// </summary>
        public DateTime DataAlteracao { get; set; }

        /// <summary>
        /// Versão do usuário.
        /// </summary>
        public int Versao { get; set; }

        /// <summary>
        /// Indica se o usuário foi excluído (true) ou não (false).
        /// </summary>

        [JsonIgnore]
        public bool VoExcluido { get; set; }

        /// <summary>
        /// Número de celular do usuário.
        /// </summary>
        public string Celular { get; set; }

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        public string Senha { get; set; }

        /// <summary>
        /// Primeiro nome do usuário.
        /// </summary>
        public string PrimeiroNome { get; set; }

        /// <summary>
        /// Identificador do usuário pai.
        /// </summary>
        public int IdUsuarioPai { get; set; }

        /// <summary>
        /// Status do usuário.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Número de convites ativos do usuário.
        /// </summary>
        public int ConvitesAtivos { get; set; }

        /// <summary>
        /// Código para alteração de senha.
        /// </summary>
        public string CodigoAlteracaoSenha { get; set; }

        /// <summary>
        /// Tipo de conta do usuário.
        /// </summary>
        public TipoContaEnum TipoConta { get; set; }
    }

   
}

