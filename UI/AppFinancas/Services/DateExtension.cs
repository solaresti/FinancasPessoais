using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace AppFinancas.Services
{
    /// <summary>
    /// Classe de extensão de datas
    /// </summary>
    public static class DateExtension
    {
        /// <summary>
        /// Troca o valor nulo de data para o valor vazio
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToStringNotNull(this DateTime? s)
        {
            if (s == null)
            {
                return "";
            }

            return s.Value.ToString("dd/MM/yyyy") ;
        }
    }


}
