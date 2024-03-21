using ApiFinancas.Enums;

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


        public class RetornoEntidadeModel<T> : RetornoBaseModel where T : EntidadeBaseModel
    {
        /// <summary>
        /// Entidade criada
        /// </summary>
        public T Entidade { get; set; }

    }


    /// <summary>
    /// Retorno com a lista de entidades
    /// </summary>
    public class RetornoListaEntidadesModel<T> : RetornoBaseModel where T: class
    {
        /// <summary>
        /// Entidade criada
        /// </summary>
        public List<T> ListaEntidades { get; set; }

    }
  

}
