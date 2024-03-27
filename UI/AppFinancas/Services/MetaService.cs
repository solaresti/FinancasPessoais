using ApiFinancas.Models;
using AppFinancas.Models;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace AppFinancas.Services
{
    /// <summary>
    /// Classe de serviços para as metas
    /// </summary>
    public static class MetaService
    {

        /// <summary>
        /// Consome o serviço para obter a lista de metas
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoListaEntidadesModel<MetaModel>> ObterLista(string token)
        {
            RetornoListaEntidadesModel<MetaModel> retornoApi = new RetornoListaEntidadesModel<MetaModel>();
            try
            {
                
                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Meta/v1", Method.Get);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro na obtenção da lista de metas (3E79FB96).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<MetaModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<MetaModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção da lista de metas (E0999798): {ex.Message}";
                return retornoApi;
            }
        }

        /// <summary>
        /// M
        /// </summary>
        /// <param name="token"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<RetornoBaseModel> ExcluirMeta(string token, int id)
        {
            RetornoBaseModel retornoApi = new RetornoBaseModel();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Meta/v1/{id}", Method.Delete);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro exclusão da meta (EEB1FDA5).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoBaseModel>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoBaseModel>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na exclusão da meta (3B8C82BF): {ex.Message}";
                return retornoApi;
            }
        }

        /// <summary>
        /// Consome o serviço para incluir uma meta
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoEntidadeModel<MetaModel>> IncluirOuAlterarMeta(string token, MetaModel model, bool flagInclusao)
        {
            RetornoEntidadeModel<MetaModel> retornoApi = new RetornoEntidadeModel<MetaModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                RestRequest request;
                if (flagInclusao)
                {
                    request = new RestRequest($"Meta/v1", Method.Post);
                }
                else
                {
                    request = new RestRequest($"Meta/v1/{model.Id}", Method.Put);
                }

                request.AddHeader("token", token);
                request.AddBody(model);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro inclusão/alteração da meta (29F4B305).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<MetaModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<MetaModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na inclusão/alteração da meta (29C66F18): {ex.Message}";
                return retornoApi;
            }
        }

        public static async Task<RetornoEntidadeModel<MetaModel>> ObterMeta(string token, int id)
        {
            RetornoEntidadeModel<MetaModel> retornoApi = new RetornoEntidadeModel<MetaModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Meta/v1/{id}", Method.Get);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro na obtenção da meta (BAFAA533).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<MetaModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<MetaModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção da meta(70281DCA): {ex.Message}";
                return retornoApi;
            }
        }

    }
}
