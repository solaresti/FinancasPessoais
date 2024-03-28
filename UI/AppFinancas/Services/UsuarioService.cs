using ApiFinancas.Models;
using AppFinancas.Models;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace AppFinancas.Services
{
    public static class UsuarioService
    {

        /// <summary>
        /// Faz o login do usuário e obtem o token de sessão
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoBaseModel> ObterToken(RequisicaoLoginModel loginModel)
        {
            RetornoBaseModel retornoLogin = new RetornoBaseModel();
            string mensagemErro = "";
            // Consumir o serviço
            string urlBaseServico = System.Environment.GetEnvironmentVariable("UrlApiFutturo");
            var client = new RestClient(urlBaseServico);

            var request = new RestRequest($"Usuario/v1/ObterToken", Method.Get);
            request.AddHeader("login", loginModel.Login);
            request.AddHeader("senha", loginModel.Senha);
            RestResponse response = client.Execute(request);


            if (!response.IsSuccessStatusCode)
            {
                string mensagemErroApi = "";
                if (string.IsNullOrWhiteSpace(response.Content))
                {
                    retornoLogin.Mensagem = $"Erro na obtenção do token de usuário (CF177400).";
                    return retornoLogin;
                }
                else
                {
                    retornoLogin = JsonConvert.DeserializeObject<RetornoBaseModel>(response.Content);
                    return retornoLogin;
                }

            }

            try
            {
                retornoLogin = JsonConvert.DeserializeObject<RetornoBaseModel>(response.Content);
                return retornoLogin;
            }
            catch (Exception ex)
            {
                retornoLogin.Mensagem = $"Erro na obtenção do token de usuário (9197BFA1): {ex.Message}";
                return retornoLogin;
            }
        }
    }
}
