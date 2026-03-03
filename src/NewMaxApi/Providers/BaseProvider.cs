using NewMaxApi.Utilities;
using System;
using System.Net;
using System.Net.Http;

namespace NewMaxApi.Providers
{
    public abstract class BaseProvider(HttpClient httpClient)
    {
        protected abstract string PathTemplate { get; }

        /// <summary>
        /// Создает HttpClient без cookies и с настраиваемым таймаутом.
        /// </summary>
        public static HttpClient CreateHttpClientWithoutCookies(TimeSpan? timeout = null)
        {
            var handler = new HttpClientHandler()
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip |
                                         DecompressionMethods.Deflate |
                                         DecompressionMethods.Brotli
            };

            var client = new HttpClient(handler);

            // По умолчанию можно оставить 100 сек, но лучше явно задать.
            client.Timeout = timeout ?? TimeSpan.FromSeconds(100);

            return client;
        }

        protected HttpClientHelper HttpClientMax { get; } = new HttpClientHelper(httpClient);
    }
}