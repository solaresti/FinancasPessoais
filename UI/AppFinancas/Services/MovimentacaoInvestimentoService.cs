﻿using ApiFinancas.Models;
using AppFinancas.Models;
using Newtonsoft.Json;
using RestSharp;
using static System.Net.WebRequestMethods;

namespace AppFinancas.Services
{
    /// <summary>
    /// Classe de serviços para as movimentações de investimentos e metas
    /// </summary>
    public static class MovimentacaoInvestimentoService
    {

        /// <summary>
        /// Consome o serviço para obter a lista de metas
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoListaEntidadesModel<ExtratoModel>> ObterExtrato(string token, int id, bool flagMeta)
        {
            RetornoListaEntidadesModel<ExtratoModel> retornoApi = new RetornoListaEntidadesModel<ExtratoModel>();
            try
            {
                
                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);

                RestRequest request;
                if (flagMeta)
                {
                    request = new RestRequest($"MovimentacaoInvestimento/v1/ExtratoMeta/{id}", Method.Get);
                }
                else
                {
                    request = new RestRequest($"MovimentacaoInvestimento/v1/ExtratoInvestimento/{id}", Method.Get);
                }
                
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

        public static async Task<RetornoListaEntidadesModel<PosicaoModel>> ObterPosicao(string token, int id, bool flagMeta)
        {
            RetornoListaEntidadesModel<PosicaoModel> retornoApi = new RetornoListaEntidadesModel<PosicaoModel>();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                RestRequest request;
                if (flagMeta)
                {
                    request = new RestRequest($"MovimentacaoInvestimento/v1/PosicaoMeta/{id}", Method.Get);
                }
                else
                {
                    request = new RestRequest($"MovimentacaoInvestimento/v1/PosicaoInvestimento/{id}", Method.Get);
                }
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro na obtenção da lista de movimentações (AAD97FB8).";
                        return retornoApi;
                    }
                    else
                    {
                        retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<PosicaoModel>>(response.Content);
                        return retornoApi;
                    }

                }

                retornoApi = JsonConvert.DeserializeObject<RetornoListaEntidadesModel<PosicaoModel>>(response.Content);
                return retornoApi;
            }
            catch (Exception ex)
            {
                retornoApi.Mensagem = $"Erro na obtenção da lista de movimentações (5395DA72): {ex.Message}";
                return retornoApi;
            }
        }


        /// <summary>
        /// Consome o serviço para obter a lista de metas
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        public static async Task<RetornoBaseModel> ExcluirMovimentacao(string token, int id)
        {
            RetornoBaseModel retornoApi = new RetornoBaseModel();
            try
            {

                string mensagemErro = "";
                // Consumir o serviço
                string urlBaseServico = "https://localhost:7076";
                var client = new RestClient(urlBaseServico);


                var request = new RestRequest($"MovimentacaoInvestimento/v1/{id}", Method.Delete);
                request.AddHeader("token", token);
                RestResponse response = client.Execute(request);


                if (!response.IsSuccessStatusCode)
                {
                    string mensagemErroApi = "";
                    if (string.IsNullOrWhiteSpace(response.Content))
                    {
                        retornoApi.Mensagem = $"Erro exclusão da movimentação (C8430209).";
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
                retornoApi.Mensagem = $"Erro na obtenção da lista de movimentações (96944458): {ex.Message}";
                return retornoApi;
            }
        }

    }
}
