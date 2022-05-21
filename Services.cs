using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiDog.Dto;

namespace DesktopApp
{
    public static class Services
    {
        public const string AddressApi = "192.168.3.254:8080";
        //public const string AddressApi = "localhost:5000";
        public static string? Token { get; set; }
        public static string PasswordToMd5(string password)
        {
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] passwordEnc = md5.ComputeHash(passwordBytes);
            string result="";
            foreach (var b in passwordEnc)
            {
                result += b.ToString("x2");
            }
            return result;
        }

        public static string? GetTokenFromServer(AccountDto user)
        {
            string? token = null;

            HttpClient httpClient = new HttpClient(){Timeout = TimeSpan.FromSeconds(5)};

            var json = JsonSerializer.Serialize(new AccountDto {Login = user.Login, PasswordHash = user.PasswordHash});
            HttpContent sendData = new StringContent(json);
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{AddressApi}/Login"),
                Content = sendData,
            };
            request.Content.Headers.Clear();
            request.Content.Headers.Add("Content-Type", "application/json");
            
            HttpResponseMessage responseMessage = httpClient.SendAsync(request).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseStream = responseMessage.Content.ReadAsStringAsync().Result;
                token = responseStream;
            
            }
            return token;
        }
        
        public static UserAccount GetRoleAndName()
        {
            UserAccount account = new UserAccount();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(Token);
            var tokenS = jsonToken as JwtSecurityToken;
            
            foreach (var VARIABLE in tokenS.Claims)
            {
                if (VARIABLE.Type == "nameid")
                    account.Role = VARIABLE.Value;
                if (VARIABLE.Type == "name")
                    account.Name = VARIABLE.Value;
            }
            return account;
        }
        
        public static T? GetData<T>(IHttpClientFactory httpClientFactory, HttpMethod httpMethod, Uri address, string dataJson="")
        {
            
            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = address
            };

            if (!string.IsNullOrEmpty(dataJson))
            {
                HttpContent sendData = new StringContent(dataJson);
                request.Content = sendData;
                request.Content.Headers.Clear();
                request.Content.Headers.Add("Content-Type", "application/json");
            }

            request.Headers.Add("Authorization", $"Bearer {Services.Token}");

            T? result = default;
            
            var client = httpClientFactory.CreateClient();

            var responseMessage = client.SendAsync(request).Result;

            if (!responseMessage.IsSuccessStatusCode) return result;
            
            using var responseStream = responseMessage.Content.ReadAsStreamAsync().Result;
                
            result = JsonSerializer.DeserializeAsync<T>(responseStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }).Result;

            return result;
        }
        
        public static async  Task<T?> GetDataAsync<T>(IHttpClientFactory httpClientFactory, HttpMethod httpMethod, Uri address, string dataJson="")
        {
            
            var request = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = address
            };

            if (!string.IsNullOrEmpty(dataJson))
            {
                HttpContent sendData = new StringContent(dataJson);
                request.Content = sendData;
                request.Content.Headers.Clear();
                request.Content.Headers.Add("Content-Type", "application/json");
            }

            request.Headers.Add("Authorization", $"Bearer {Services.Token}");

            T? result = default;
            
            var client = httpClientFactory.CreateClient();

            var responseMessage = await client.SendAsync(request);

            if (!responseMessage.IsSuccessStatusCode) return result;
            
            using var responseStream = await responseMessage.Content.ReadAsStreamAsync();
                
            result = await JsonSerializer.DeserializeAsync<T>(responseStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}