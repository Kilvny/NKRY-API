using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace NKRY_API.Utilities
{
    public static class QRCodeGenerator
    {
        public async static Task<string> GenerateAsync(string data, int dimensions = 150) 
        {
            string qrURL = await QRApiService.GetQRUrl(data, dimensions);
            return qrURL;
        }
    }

    public static class QRApiService
    {
        private static readonly HttpClient client;
        
        static QRApiService()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri("https://api.qrserver.com")
            };
        }

        public async static Task<string> GetQRUrl(string data, int dimensions) 
        {
            var url = $"/v1/create-qr-code/?size={dimensions}x{dimensions}&data={data}";
            var result = "";
            var response = await client.GetAsync(url);

            var QrUrl = $"{client.BaseAddress}{url}";

            if (response.IsSuccessStatusCode)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();

                result = stringResponse;
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return result;
            // return QrUrl;
        } 
    }
}