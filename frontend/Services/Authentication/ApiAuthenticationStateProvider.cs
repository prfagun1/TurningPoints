using Blazored.LocalStorage;
using entities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Security.Claims;
using System.Text.Json;

namespace frontend.Services
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private VariaveisAmbiente _variaveisAmbiente;

        public ApiAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorage, VariaveisAmbiente variaveisAmbiente)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _variaveisAmbiente = variaveisAmbiente;
        }

        public void MarUserAuthenticated(string usuario, string token)
        {
            IEnumerable<Claim> claims = ParseClaimsFromJwt(token).ToList();
            //claims.Add(new Claim(ClaimTypes.Name, usuario));

            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
            //var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario) }, "apiauth"));


            ParseClaimsFromJwt(token);

            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));

            ParseClaimsFromJwt(token);

            NotifyAuthenticationStateChanged(authState);
        }

        public void MarUserAsLoggedOut()
        {
            var annonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(annonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes) ?? throw new ArgumentException("Não foi possível obter parâmetros do tojen");
            keyValuePairs.TryGetValue("role", out object? roles);


            if (roles != null)
            {
                string rolesString = roles.ToString() ?? string.Empty;

                if (rolesString.Trim().StartsWith("["))
                {
                    var parseRoles = JsonSerializer.Deserialize<string[]>(rolesString);
                    if (parseRoles != null)
                        foreach (var parseRole in parseRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, parseRole));
                        }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, rolesString));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            string unique_name = keyValuePairs["unique_name"].ToString() ?? string.Empty;

            claims.AddRange(keyValuePairs.Select(x => new Claim(x.Key, x.Value.ToString() ?? string.Empty)));

            claims.Add(new Claim(ClaimTypes.Name, unique_name));

            return claims;
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 0: break; // No pad chars in this case
                case 2: base64 += "=="; break; // Two pad chars
                case 3: base64 += "="; break; // One pad char
                default: throw new System.ArgumentOutOfRangeException("input", "Illegal base64url string!");
            }

            var converted = Convert.FromBase64String(base64);
            return converted;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            LoginResult? savedToken;
            try
            {
                savedToken = await _localStorage.GetItemAsync<LoginResult>("authToken");
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            
            //Verifica expiracao do token
            if (savedToken == null )
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(savedToken.Token);

            if (!jwtToken.Issuer.Equals("TurningPoints"))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            if (jwtToken?.ValidTo < DateTime.UtcNow)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            if (string.IsNullOrEmpty(savedToken.Token))
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var authenticationState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(savedToken.Token), "jwt")));
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", savedToken.Token);
            _httpClient.Timeout = TimeSpan.FromMinutes(120);
            _variaveisAmbiente.Usuario = authenticationState?.User?.Identity?.Name;
            _variaveisAmbiente.Token = savedToken.Token;
            _variaveisAmbiente.DataExpiracao = savedToken.ExpirationDate;

            //Console.WriteLine($"estou autenticado 1 {_variaveisAmbiente.Usuario}");

            if (authenticationState == null)
                throw new Exception("Erro ao realizar processo de autenticação");

            return authenticationState;

        }
    }
}
