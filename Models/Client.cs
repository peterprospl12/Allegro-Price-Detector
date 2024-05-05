using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
namespace Allegro_Price_Detector.Models;

    public class Client
    {
        private readonly string _clientId = "";
        private readonly string _clientSecret = "";
        private readonly string _tokenUrl = "https://allegro.pl.allegrosandbox.pl/auth/oauth/token";
        private string accessToken = "";
        private readonly HttpClient _httpClient;

        public Client()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.allegro.public.v1+json");
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Allegro API Client");
            InitializeAsync();
        }
        
        private async void InitializeAsync()
        {
            accessToken = await GetAccessTokenAsync();
        }

        public HttpClient getHttpClient()
        {
            return _httpClient;
        }

        public string getToken()
        {
            return accessToken;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var formData = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    });

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic",
                        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_clientId}:{_clientSecret}")));

                    var response = await client.PostAsync(_tokenUrl, formData);
                    response.EnsureSuccessStatusCode();

                    var tokens = await response.Content.ReadAsAsync<TokenResponse>();
                    Console.WriteLine("Access token given");
                    return tokens.access_token;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        
        private class TokenResponse
        {
            public string access_token { get; set; }
        }
    

    }
