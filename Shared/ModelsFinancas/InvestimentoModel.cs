using ApiFinancas.Enums;
using System.ComponentModel.DataAnnotations;

namespace ApiFinancas.Models
{
    public class InvestimentoModel : EntidadeBaseModel
    {
        /// <summary>
        /// Obtém ou define o nome do investimento.
        /// </summary>
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Nome { get; set; }


        /// <summary>
        /// Obtém ou define a data de vencimento do investimento (pode ser nulo).
        /// </summary>
        public DateTime? Vencimento { get; set; }

        /// <summary>
        /// Obtém ou define a categoria do investimento (valor inteiro).
        /// </summary>
        public int Categoria { get; set; }

        /// <summary>
        /// Obtém ou define o nível de risco do investimento (valor inteiro).
        /// </summary>
        public int Risco { get; set; }


        /// <summary>
        /// Nome da categoria (read-only)
        /// </summary>
        public string NomeCategoria
        {
            get
            {
                switch (Categoria)
                {
                    //case CategoriaInvestimentoEnum.RendaFixa:
                    //    return "Renda Fixa";

                    //case CategoriaInvestimentoEnum.RendaVariavel:
                    //    return "Renda Variável";

                    //case CategoriaInvestimentoEnum.Cambial:
                    //    return "Cambial";

                    //case CategoriaInvestimentoEnum.Criptomoedas:
                    //    return "Criptomoeda";

                    //case CategoriaInvestimentoEnum.MultiMercados:
                    //    return "Multimercado";

                    case 1:
                        return "Renda Fixa";

                    case 2:
                        return "Renda Variável";

                    case 3:
                        return "Cambial";

                    case 4:
                        return "Criptomoeda";

                    case 5:
                        return "Multimercado";
                    default:
                        return "Categoria não definida"; ;
                }

            }
        }

        /// <summary>
        /// Nome do risco (read-only)
        /// </summary>
        public string NomeRisco
        {
            get
            {
                switch (Risco)
                {
                    

                    case 1:
                        return "Baixo";

                    case 2:
                        return "Médio";

                    case 3:
                        return "Alto";

                    default:
                        return "Risco não definido"; ;
                }

            }
        }

    }
}

