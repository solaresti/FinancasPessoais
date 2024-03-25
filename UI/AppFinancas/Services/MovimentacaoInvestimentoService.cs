using ApiFinancas.Models;
using AppFinancas.Models;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace AppFinancas.Services
{
    /// <summary>
    /// Classe de serviços pas as movimentações de investimentos
    /// </summary>
    public static class MovimentacaoInvestimentoService
    {

        /// <summary>
        /// Consome o serviço para obter a lista de metas
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoListaEntidadesModel<ExtratoModel>> ObterExtratoPorMeta(string token, int id)
        {
            RetornoListaEntidadesModel<ExtratoModel> retornoApi = new RetornoListaEntidadesModel<ExtratoModel>();
            try
            {
                
                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"MovimentacaoInvestimento/v1/ExtratoMeta/{id}", Method.Get);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro na obtenção da lista de movimentações (122029D2).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<ExtratoModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<ExtratoModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção da lista de movimentações (FC72ADE6): {ex.Message}";
                return retornoApi;
            }
        }
    }
}
