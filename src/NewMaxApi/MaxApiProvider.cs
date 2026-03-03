using NewMaxApi.Providers;
using System;
using System.Net;
using System.Net.Http;

namespace NewMaxApi
{
    /// <summary>
    /// Предоставляет доступ к сервисам API MAX.
    /// </summary>
    public class MaxApiProvider : IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        /// <summary>
        /// Таймаут по умолчанию для API-запросов.
        /// </summary>
        public static readonly TimeSpan DefaultApiTimeout = TimeSpan.FromSeconds(60);

        public static HttpClient CreateHttpClientWithoutCookies(TimeSpan? timeout = null)
        {
            var handler = new HttpClientHandler()
            {
                UseCookies = false,
                AutomaticDecompression = DecompressionMethods.GZip |
                                         DecompressionMethods.Deflate |
                                         DecompressionMethods.Brotli
            };

            var client = new HttpClient(handler)
            {
                Timeout = timeout ?? DefaultApiTimeout
            };

            return client;
        }

        /// <summary>
        /// Инициализирует новый экземпляр MaxApiProvider.
        /// </summary>
        /// <param name="accessToken">Токен доступа.</param>
        /// <param name="httpClient">
        /// Внешний HttpClient (опционально). Если не передан — будет создан новый.
        /// </param>
        /// <param name="requestTimeout">
        /// Таймаут запросов API. Если передан внешний httpClient, таймаут будет применен к нему тоже.
        /// </param>
        public MaxApiProvider(
            string accessToken,
            HttpClient? httpClient = null,
            TimeSpan? requestTimeout = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);

            _httpClient = httpClient ?? CreateHttpClientWithoutCookies(requestTimeout);

            // Применяем таймаут даже для внешнего клиента (если указан)
            _httpClient.Timeout = requestTimeout ?? DefaultApiTimeout;

            if (_httpClient.BaseAddress == null)
                _httpClient.BaseAddress = new Uri("https://platform-api.max.ru");

            Bots = new(accessToken, _httpClient);
            Chats = new(accessToken, _httpClient);
            Subscriptions = new(accessToken, _httpClient);
            Upload = new(accessToken, _httpClient);
            Messages = new(accessToken, _httpClient);
        }

        public BotsProvider Bots { get; }
        public ChatsProvider Chats { get; }
        public SubscriptionsProvider Subscriptions { get; }
        public UploadsProvider Upload { get; }
        public MessagesProvider Messages { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _httpClient.Dispose();
                _disposed = true;
            }
        }
    }
}