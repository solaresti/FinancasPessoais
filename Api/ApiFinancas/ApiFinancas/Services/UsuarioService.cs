using ApiFinancas.Enums;
using ApiFinancas.Models;

namespace ApiFinancas.Services
{
    /// <summary>
    /// Serviços relacionado ao usuário
    /// </summary>
    public static class UsuarioService
    {

        /// <summary>
        /// Método para validação do token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="mensagemErro"></param>
        /// <returns></returns>
        public static bool ValidarToken (string token,ref int idUsuario, ref TipoContaEnum tipoUsuario, ref string mensagemErro)
        {
            string tokenAberto = "";
            bool retornoCriptografia = CriptografiaService.DecryptStringToken(token, ref tokenAberto);
            if (!retornoCriptografia)
            {
                mensagemErro = "Token inválido (E7FA2D3A).";
                return false;
            }

            string[] parametros = tokenAberto.Split ('|');

            if (parametros.Length != 3 )
            {
                mensagemErro = "Token inválido (9620E7FA).";
                return false;
            }

            try
            {
                idUsuario = Convert.ToInt32(parametros[0]);
            }
            catch (Exception)
            {
                mensagemErro = "Token inválido (689AFDDB).";
                return false;
            }

            try
            {
                tipoUsuario = (TipoContaEnum)Convert.ToInt32(parametros[1]);
            }
            catch (Exception)
            {
                mensagemErro = "Token inválido (689AFDDB).";
                return false;
            }

            DateTime dataValidadeToken;
            try
            {
                dataValidadeToken = new DateTime(Convert.ToInt64(parametros[2]));
            }
            catch (Exception)
            {
                mensagemErro = "Token inválido (689AFDDB).";
                return false;
            }

            if (dataValidadeToken < DateTime.Now)
            {
                mensagemErro = "Token inválido (1AEF42EA).";
                return false;
            }

            return true;
        }



    }
}
