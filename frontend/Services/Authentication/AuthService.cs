//continuar o video https://www.youtube.com/watch?v=YIr8Ro4G2ek 39:14
//Ver sobre metricas https://www.youtube.com/watch?v=-G-YdDECp7I 

using Blazored.LocalStorage;
using entities;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace frontend.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private VariaveisAmbiente _variaveisAmbiente;

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider, VariaveisAmbiente variaveisAmbiente)
        {
            _httpClient = httpClient;
            try
            {
                _httpClient.Timeout = TimeSpan.FromMinutes(60);
            }
            catch { }


            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _variaveisAmbiente = variaveisAmbiente;
        }

        //Verificar os models de login
        public async Task<LoginResult?> Login(LoginViewModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
                throw new ArgumentException("Os campos username e password não pode estar em branco");

            string urlLogin = _variaveisAmbiente.UrlAPI + "/v1/Login";
            var loginJson = JsonSerializer.Serialize(loginModel);

            try
            {
                var response = await _httpClient.PostAsync(urlLogin, new StringContent(loginJson, Encoding.UTF8, "application/json"));

                var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (loginResult == null || !response.IsSuccessStatusCode || !loginResult.Successful || string.IsNullOrEmpty(loginResult.Token))
                {
                    return loginResult;
                }


                await _localStorage.SetItemAsync("authToken", loginResult);
                await _localStorage.SetItemAsync("dataToken", DateTime.Now.ToString());
                await _localStorage.SetItemAsync("dataExpiracaoToken", loginResult?.ExpirationDate);
                await _localStorage.SetItemAsync("duracaoToken", _variaveisAmbiente.TokenTempoExpiracaoMinutos);

                if (string.IsNullOrEmpty(loginResult?.Token))
                    return loginResult;

                ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarUserAuthenticated(loginModel.Email, loginResult.Token);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

                return loginResult;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao logar com usuario {loginModel.Email}: {e.ToString()}");
                return null;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("dataToken");
            await _localStorage.RemoveItemAsync("duracaoToken");
            await _localStorage.RemoveItemAsync("dataExpiracaoToken");

            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

    }
}
