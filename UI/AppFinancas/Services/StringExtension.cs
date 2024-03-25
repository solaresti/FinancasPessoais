using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace AppFinancas.Services
{
    /// <summary>
    /// Classe de extensão de string
    /// </summary>
    public static class StringExtension
    {
        public static string RemoverAcentos(this String s)
        {
            String normalizedString = s.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < normalizedString.Length; i++)
            {
                Char c = normalizedString[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Troca o valor nulo de uma string para o valor vazio
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string NotNull(this String s)
        {
            if (s == null)
            {
                return "";
            }

            return s;
        }


        /// <summary>
        /// Verifica se uma string é composta apenas por dígitos
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsDigit(this String s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            for (int i = 0; i < s.Length; i++)
            {
                // bool isDigit = s.Substring(i, 1).IsDigit();
                bool isDigit = Char.IsDigit(s, i);
                if (!isDigit)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Remove todos os caracteres não numéricos de uma string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string JustDigit(this String s)
        {

            if (string.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            string retorno = "";

            for (int i = 0; i < s.Length; i++)
            {
                // bool isDigit = s.Substring(i, 1).IsDigit();
                bool isDigit = Char.IsDigit(s, i);
                if (isDigit)
                {
                    retorno += s.Substring(i, 1);
                }
            }
            return retorno;
        }

        /// <summary>
        /// Remove todos os caracteres não numéricos de uma string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsGuid(this String s)
        {

            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            string stringGuid = "";

            for (int i = 0; i < s.Length; i++)
            {
                stringGuid += s.Substring(i, 1);
            }

            Guid guidTeste;
            bool retornoTeste = Guid.TryParse(stringGuid, out guidTeste);
            return retornoTeste;
        }

        /// <summary>
        /// Verifica se um e-mai é valido
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this String s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            string stringEmail = "";

            for (int i = 0; i < s.Length; i++)
            {
                stringEmail += s.Substring(i, 1);
            }

            // Define a expressão regular para validar um endereço de e-mail
            string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";

            // Cria um objeto Regex com a expressão regular
            Regex regex = new Regex(pattern);

            // Verifica se o e-mail corresponde à expressão regular
            return regex.IsMatch(stringEmail);
        }





    }


}
