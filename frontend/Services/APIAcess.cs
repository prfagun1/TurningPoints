
using entities;
using entities.Helpers;
using entity;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace frontend.Services
{
    public class APIAcess
    {

        private readonly VariaveisAmbiente _variaveisAmbiente;
        private readonly HttpClient _httpClient;

        public APIAcess(VariaveisAmbiente variaveisAmbiente, HttpClient httpClient)
        {
            _variaveisAmbiente = variaveisAmbiente;
            _httpClient = httpClient;
        }

        private string GetURL(string versao, string api, Enumerators.APIConnection apiConexao)
        {
            if (apiConexao == Enumerators.APIConnection.api)
                return $"{_variaveisAmbiente.UrlAPI}/{versao}/{api}";
            else
                throw new ArgumentException("Necessário informar Enumeradores.APIConexao.api como parâmetro");
        }

        public async Task<List<Chapter>?> GetChapterListByGuid(string versao, string api, string storyId, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = $"{GetURL(versao, api, apiConexao)}?storyId={storyId}";

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<List<Chapter>>(retorno);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<PaginationList<T>?> BuscaDados<T>(string versao, string api, SearchFilter filtro, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = GetURL(versao, api, apiConexao);

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.PostAsJsonAsync(url, filtro);
                var jsonRetorno = await retorno.Content.ReadAsStringAsync();

                
                return JsonConvert.DeserializeObject<PaginationList<T>>(jsonRetorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public async Task<bool> BuscaDadosPermiteSalvar<T>(string versao, string api, T entidade, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = GetURL(versao, api, apiConexao);

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.PostAsJsonAsync(url, entidade);
                var jsonRetorno = await retorno.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<bool>(jsonRetorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public async Task<T?> BuscaDados<T>(string versao, string api, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = GetURL(versao, api, apiConexao);

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<byte[]?> BuscaDadosByte(string versao, string api, SearchFilter filtro, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = GetURL(versao, api, apiConexao);

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.PostAsJsonAsync(url, filtro);
                var jsonRetorno = await retorno.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<byte[]>(jsonRetorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public async Task<T?> BuscaDadosBytePorId<T>(string versao, string api, Guid id, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = $"{GetURL(versao, api, apiConexao)}/{id}";

            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> BuscaDadosPorUsuario<T>(string versao, string api, string usuario, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}/{usuario}";
            try
            {
                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> BuscaDadosPorId<T>(string versao, string api, Guid id, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}/{id}";
            try
            {
                var retorno = await _httpClient.GetAsync(url);
                if (retorno.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<T>(await retorno.Content.ReadAsStringAsync());
                }
                Console.WriteLine($"Erro ao buscar dados {retorno.StatusCode.ToString()}");
                return default;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> BuscaDadosPorId<T>(string versao, string api, long id, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}/{id}";
            try
            {
                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> BuscaDadosPorId<T>(string versao, string api, string id, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}?id={id}";
            try
            {
                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> BuscaParametros<T>(string versao, string api, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<T>(retorno);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<T?> SalvaDados<T>(string versao, string api, T objeto, Enumerators.APIConnection apiConexao)
        {


            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.PostAsJsonAsync<T>(url, objeto);
                if (retorno.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Salvo dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return JsonConvert.DeserializeObject<T>(await retorno.Content.ReadAsStringAsync());
                }
                if (retorno.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorContent = await retorno.Content.ReadAsStringAsync();
                    return default;
                }
                else
                {
                    Console.WriteLine($"Erro ao salvar dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return default;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao salvar dados da API {versao}/{api} para o objeto {typeof(T)}, {e}");
                return default;
            }
        }

        public async Task<APIResponse?> SalvaDadosRetornoAPI<T>(string versao, string api, T objeto, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.PostAsJsonAsync<T>(url, objeto);
                if (retorno.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Salvo dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return JsonConvert.DeserializeObject<APIResponse>(await retorno.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.WriteLine($"Erro ao salvar dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return JsonConvert.DeserializeObject<APIResponse>(await retorno.Content.ReadAsStringAsync());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao salvar dados da API {versao}/{api} para o objeto {typeof(T)}, {e}");
                return new APIResponse(Enumerators.APIResponseStatus.Error, $"Erro ao salvar dados", "Editar dados", 0);
            }
        }

        public async Task<bool> EditaDados<T>(string versao, string api, T objeto, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                string inputJson = JsonConvert.SerializeObject(objeto);
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _httpClient.PutAsync(url, inputContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Editado dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return true;
                }
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return default;
                }
                else
                {
                    Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}, {e.ToString()}");
                return false;
            }
        }

        public async Task<bool> EditaDadosAsync<T>(string versao, string api, T objeto, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                string inputJson = JsonConvert.SerializeObject(objeto);
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync(url, inputContent);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Editado dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return true;
                }

                else
                {
                    Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}, {e.ToString()}");
                return false;
            }
        }

        public async Task<APIResponse?> EditaDadosRetornoAPI<T>(string versao, string api, T objeto, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                string inputJson = JsonConvert.SerializeObject(objeto);
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync(url, inputContent);


                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Editado dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());
                }

                else
                {
                    Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}");
                    return JsonConvert.DeserializeObject<APIResponse>(await response.Content.ReadAsStringAsync());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao editar dados da API {versao}/{api} para o objeto {typeof(T)}, {e.ToString()}");
                return new APIResponse(Enumerators.APIResponseStatus.Error, $"Erro ao editar dados, {e}", "Editar dados", 0);
            }
        }

        public async Task<Boolean> Apagar(string versao, string api, Guid id, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.DeleteAsync($"{url}/{id}");
                if (retorno.IsSuccessStatusCode)
                    return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return false;
        }

        public async Task<Boolean> Apagar(string versao, string api, string id, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.DeleteAsync($"{url}/{id}");
                if (retorno.IsSuccessStatusCode)
                    return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return false;
        }

        public async Task<APIResponse?> CalculaNotasSistemas()
        {
            string url = $"{_variaveisAmbiente.UrlAPI}/v1/Calculo/CalculaNotasSistemas";
            try
            {
                var retorno = await _httpClient.GetStringAsync(url);
                var retornoAPI = JsonConvert.DeserializeObject<APIResponse>(retorno);

                if (retornoAPI?.APIResponseStatus == Enumerators.APIResponseStatus.Error)
                {
                    Console.WriteLine($"Erro no processo {retornoAPI.Request}: {retornoAPI.Message}");
                }
                return retornoAPI;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }
        }

        public async Task<APIResponse?> ExecutaProcedimentoGet(string versao, string api, Enumerators.APIConnection apiConexao)
        {

            string url = GetURL(versao, api, apiConexao);
            try
            {
                var retorno = await _httpClient.GetStringAsync(new Uri(url));
                var retornoAPI = JsonConvert.DeserializeObject<APIResponse>(retorno);

                if (retornoAPI?.APIResponseStatus == Enumerators.APIResponseStatus.Error)
                {
                    Console.WriteLine($"Erro no processo {retornoAPI.Request}: {retornoAPI.Message}");
                }
                return retornoAPI;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public async Task<APIResponse?> ExecutaProcedimentoPut(string versao, string api, string usuario, Enumerators.APIConnection apiConexao, double? timeout = null)
        {

            string url = GetURL(versao, api, apiConexao);
            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                string inputJson = JsonConvert.SerializeObject(usuario);
                HttpContent inputContent = new StringContent(inputJson, Encoding.UTF8, "application/json");
                var respoonse = _httpClient.PutAsync(url, inputContent).Result;

                var retornoAPI = JsonConvert.DeserializeObject<APIResponse>(await respoonse.Content.ReadAsStringAsync());

                if (retornoAPI?.APIResponseStatus == Enumerators.APIResponseStatus.Error)
                {
                    Console.WriteLine($"Erro no processo {retornoAPI.Request}: {retornoAPI.Message}");
                }

                return retornoAPI;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public async Task<APIResponse?> ExecutaProcedimentoGet(string versao, string api, int id, Enumerators.APIConnection apiConexao, double? timeout = null)
        {
            string url = GetURL(versao, api, apiConexao) + "/" + id;
            try
            {
                if (timeout != null)
                {
                    _httpClient.Timeout = TimeSpan.FromMinutes(timeout.Value);
                }

                var retorno = await _httpClient.GetStringAsync(url);
                var retornoAPI = JsonConvert.DeserializeObject<APIResponse>(retorno);

                if (retornoAPI?.APIResponseStatus == Enumerators.APIResponseStatus.Error)
                {
                    Console.WriteLine($"Erro no processo {retornoAPI.Request}: {retornoAPI.Message}");
                }


                return retornoAPI;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return default;
            }

        }

        public async Task<string> ExecutaAPI<T>(string versao, string api, T entidade, Enumerators.APIConnection apiConexao)
        {

            string url = $"{GetURL(versao, api, apiConexao)}";
            try
            {
                var retorno = await _httpClient.PostAsJsonAsync(url, entidade);
                var jsonRetorno = await retorno.Content.ReadAsStringAsync();

                if (retorno.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Executado processo via API {versao}/{api} para o objeto {typeof(T)}");
                    return jsonRetorno;
                }

                else
                {
                    string mensagem = $"Erro ao executar processo via API {versao}/{api} para o objeto {typeof(T)}, mensagem retornada: {jsonRetorno}";
                    Console.WriteLine(mensagem);
                    return mensagem;
                }

            }
            catch (Exception e)
            {
                string mensagem = $"Erro ao executar processo via API {versao}/{api} para o objeto {typeof(T)}, erro: {e}";
                Console.WriteLine(mensagem);
                return mensagem;
            }
        }
    }
}
