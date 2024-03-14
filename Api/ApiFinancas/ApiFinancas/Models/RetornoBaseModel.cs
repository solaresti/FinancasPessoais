namespace ApiFinancas.Models
{
    /// <summary>
    /// Classe de retorno padrão da API
    /// </summary>
    public class RetornoBaseModel
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public RetornoBaseModel()
        {
            DataRetorno = DateTime.Now;
        }

        /// <summary>
        /// Código do retorno
        /// </summary>
        public CodigosRetornoEnum Codigo { get; set; }

        /// <summary>
        /// Mensagem do retorno
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// Data do retorno
        /// </summary>
        public DateTime DataRetorno { get; set; }

    }

    public class RetornoUsuarioModel : RetornoBaseModel
    {
        /// <summary>
        /// Entidade criada
        /// </summary>
        public UsuarioModel Entidade { get; set; }

    }

    /// <summary>
    /// Retorno com a lista de usuários
    /// </summary>
    public class RetornoListaUsuariosModel : RetornoBaseModel
    {
        /// <summary>
        /// Entidade criada
        /// </summary>
        public List<UsuarioModel> ListaEntidades { get; set; }

    }

    public enum CodigosRetornoEnum
    {
        Sucesso=1,
        Excecao=-1,
        NaoLocalizado=-2
    }

}
