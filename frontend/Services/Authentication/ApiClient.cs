﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace frontend.Services.Authentication
{
    public class ApiClient(HttpClient httpClient, ProtectedLocalStorage localStorage, AuthenticationStateProvider authStateProvider)
    {
        public async Task SetAuthorizeHeader()
        {
            var sessionState = (await localStorage.GetAsync<LoginResponseModel>("sessionState")).Value;
            if (sessionState != null && !string.IsNullOrEmpty(sessionState.Token))
            {
                if (sessionState.TokenExpired < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                {
                    await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                }
                else if (sessionState.TokenExpired < DateTimeOffset.UtcNow.AddMinutes(10).ToUnixTimeSeconds())
                {
                    var res = await httpClient.GetFromJsonAsync<LoginResponseModel>($"/api/auth/loginByRefeshToken?refreshToken={sessionState.RefreshToken}");
                    if (res != null)
                    {
                        await ((CustomAuthStateProvider)authStateProvider).MarUserAuthenticated(res);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.Token);
                    }
                    else
                    {
                        await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                    }
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionState.Token);
                }
            }
        }
        public async Task<T> GetFromJsonAsync<T>(string path)
        {
            await SetAuthorizeHeader();
            return await httpClient.GetFromJsonAsync<T>(path);
        }
        public async Task<T1> PostAsync<T1, T2>(string path, T2 postModel)
        {
            await SetAuthorizeHeader();

            var res = await httpClient.PostAsJsonAsync(path, postModel);
            if (res != null && res.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
            }
            return default;
        }
        public async Task<T1> PutAsync<T1, T2>(string path, T2 postModel)
        {
            await SetAuthorizeHeader();
            var res = await httpClient.PutAsJsonAsync(path, postModel);
            if (res != null && res.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync());
            }
            return default;
        }
        public async Task<T> DeleteAsync<T>(string path)
        {
            await SetAuthorizeHeader();
            return await httpClient.DeleteFromJsonAsync<T>(path);
        }
    }
}
