using AppFinancas.Services;
using System.ComponentModel.DataAnnotations;

namespace AppFinancas.Models
{
    /// <summary>
    /// Classe de dados para login
    /// </summary>
    public class RequisicaoLoginModel
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        [Required(ErrorMessage = "Informe o e-mail do usuário")]
        private string login;
        public string Login
        {
            get { return login; }
            set { login = value.NotNull().Trim(); }
        }



        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessage = "Informe a senha do usuário.")]
        public string Senha { get; set; }

    }
}
