namespace ApiFinancas.Models
{
    /// <summary>
    /// Model de Posição de uma determinada meta
    /// </summary>
    public class PosicaoPorMetaModel
    {
        /// <summary>
        /// Obtém ou define o valor de uma movimentação de investimento.
        /// </summary>
        public float Valor { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da meta.
        /// </summary>
        public int IdInvestimento { get; set; }

        /// <summary>
        /// Nome do investimento
        /// </summary>
        public string Nome { get; set; }
    }
    
}