using MIST_infrastructure;
using MIST_infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MIST_app.Services
{
    public class HttpService
    {
        private static HttpService _instance = null;
        public static HttpService Instance
        {
            get
            {
                return _instance ?? (_instance = new HttpService(DeviceInfo.Platform == DevicePlatform.Android ? "10.0.2.2" : "localhost", "7208"));
            }
        }

        public string Port { get; }
        public string BaseAddress { get; }
        public string Url { get; }
        public HttpClient client { get; }
        private HttpService(string address, string port)
        {
            BaseAddress = address;
            Port = port;
            Url = $"https://{BaseAddress}:{Port}";
            client = new HttpClient(GetInsecureHandler());
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
        }
        private HttpClientHandler GetInsecureHandler()
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                if (cert.Issuer.Equals("CN=localhost"))
                    return true;
                return errors == System.Net.Security.SslPolicyErrors.None;
            };

            return handler;
        }

        public async Task<T> Get<T>(string api)
        {
            var str = await client.GetStringAsync(Url + api);
            T deserialized = JsonService.Instance.Deserialize<T>(str);
            return deserialized;
        }
        public async Task<TransferObj<T>> GetTrObj<T>(string api)
        {
            var str = await client.GetStringAsync(Url + api);
            TransferObj<T> deserialized = JsonService.Instance.Deserialize<TransferObj<T>>(str);
            return deserialized;
        }


        public async Task<TransferObj<T>> Post<T>(string api, HttpContent content)
        {
            var response = await client.PostAsync(Url + api, content);

            string serialized = await response.Content.ReadAsStringAsync();


            return JsonService.Instance.Deserialize<TransferObj<T>>(serialized);
        }
    }
}
