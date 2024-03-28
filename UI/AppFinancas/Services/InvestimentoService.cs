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
    public static class InvestimentoService
    {

        /// <summary>
        /// Consome o serviço para obter a lista de metas
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoListaEntidadesModel<InvestimentoModel>> ObterLista(string token)
        {
            RetornoListaEntidadesModel<InvestimentoModel> retornoApi = new RetornoListaEntidadesModel<InvestimentoModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = System.Environment.GetEnvironmentVariable("UrlApiFutturo");
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Investimento/v1", Method.Get);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro na obtenção da lista de investimentos (482B0C3C).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<InvestimentoModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<InvestimentoModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção da lista de investimentos (B5C262C5): {ex.Message}";
                return retornoApi;
            }
        }

        /// <summary>
        /// Consome o serviço excluir um investimento
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoBaseModel> ExcluirInvestimento(string token, int id)
        {
            RetornoBaseModel retornoApi = new RetornoBaseModel();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = System.Environment.GetEnvironmentVariable("UrlApiFutturo");
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Investimento/v1/{id}", Method.Delete);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro exclusão do investimento (1D59964B).";
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
                retornoApi.Mensagem = $"Erro na exclusão do investimento (705449C6): {ex.Message}";
                return retornoApi;
            }
        }

        /// <summary>
        /// Consome o serviço excluir um investimento
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoEntidadeModel<InvestimentoModel>> IncluirOuAlterarInvestimento(string token, InvestimentoModel model, bool flagInclusao)
        {
            RetornoEntidadeModel<InvestimentoModel> retornoApi = new RetornoEntidadeModel<InvestimentoModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = System.Environment.GetEnvironmentVariable("UrlApiFutturo");
                var client = new RestClient(urlBaseServico);


                RestRequest request;
                if (flagInclusao)
                {
                    request  = new RestRequest($"Investimento/v1", Method.Post);
                }
                else
                {
                    request = new RestRequest($"Investimento/v1/{model.Id}", Method.Put);
                }

                request.AddHeader("token", token);
                request.AddBody(model);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro inclusão/alteração do investimento (29F4B305).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<InvestimentoModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<InvestimentoModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na inclusão/alteração do investimento (29C66F18): {ex.Message}";
                return retornoApi;
            }
        }

        /// <summary>
        /// Consome o serviço excluir um investimento
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoEntidadeModel<InvestimentoModel>> ObterInvestimento(string token, int id)
        {
            RetornoEntidadeModel<InvestimentoModel> retornoApi = new RetornoEntidadeModel<InvestimentoModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = System.Environment.GetEnvironmentVariable("UrlApiFutturo");
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"Investimento/v1/{id}", Method.Get);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro obtenção do investimento (71468907).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<InvestimentoModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoEntidadeModel<InvestimentoModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção do investimento (827C6BC1): {ex.Message}";
                return retornoApi;
            }
        }
    }
}
