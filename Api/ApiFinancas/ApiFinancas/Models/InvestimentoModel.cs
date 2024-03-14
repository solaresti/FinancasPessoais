namespace ApiFinancas.Models
{
    public class InvestimentoModel
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
        /// Obtém ou define o nome do investimento.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Obtém ou define a data de vencimento do investimento (pode ser nulo).
        /// </summary>
        public DateTime? Vencimento { get; set; }

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
        public bool VoExcluido { get; set; }

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

