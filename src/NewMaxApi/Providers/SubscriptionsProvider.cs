using NewMaxApi.Providers;
using NewMaxApi.Requests.Subscriptions;
using NewMaxApi.Resources;
using NewMaxApi.Responses;
using NewMaxApi.Responses.Subscriptions;
using System.Linq;
using System.Net.Http;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace NewMaxApi
{
    /// <summary>
    /// Предоставляет методы для взаимодействия с конечными точками, связанными с подпиской, в API MAX.
    /// </summary>
    /// <param name="accessToken">Токен доступа, используемый для аутентификации API.</param>
    /// <param name="httpClient">Экземпляр <see cref="HttpClient"/>, используемый для запросов API.</param>
    public partial class SubscriptionsProvider(string accessToken, HttpClient httpClient) : BaseProvider(httpClient)
    {
        protected override string PathTemplate => "/subscriptions";

        /// <summary>
        /// Если ваш бот получает данные через WebHook, этот метод возвращает список всех подписок.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<GetSubscriptionsResponse> GetSubscriptionsAsync(CancellationToken cancellationToken = default)
        {
            return await HttpClientMax.GetAsync<GetSubscriptionsResponse>($"{PathTemplate}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Подписывает бота на получение обновлений через WebHook. После вызова этого метода бот будет получать уведомления о новых событиях чата по указанному URL-адресу.
        /// </summary>
        /// <remarks>
        /// Ваш сервер должен прослушивать один из следующих портов: 80, 8080, 443, 8443, 16384-32383.
        /// </remarks>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> SubscribeAsync(SubscribeRequest body, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);

            if (!SecretRegex().IsMatch(body.Secret))
                throw new ArgumentException(nameof(body.Secret), Error.SecretRegexError);

            return await HttpClientMax.PostAsync<SuccessResponse>($"{PathTemplate}?access_token={accessToken}", body, cancellationToken);
        }

        /// <summary>
        /// Отписывает бота от получения обновлений через WebHook. После вызова этого метода бот перестаёт получать уведомления о новых событиях, и доступна доставка уведомлений через API с длинным опросом.
        /// </summary>
        /// <param name="url">URL для удаления из подписки на WebHook.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> UnsubscribeAsync(string url, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(url);
            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}?url={url}&access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Этот метод можно использовать для получения обновлений, если ваш бот не подписан на WebHook. Метод использует длинный опрос.
        /// </summary>
        /// <remarks>
        /// Каждое обновление имеет свой порядковый номер. Свойство <paramref name="marker"/> в ответе указывает на следующее ожидаемое обновление. Все предыдущие обновления считаются выполненными после передачи параметра <paramref name="marker"/>. Если параметр <paramref name="marker"/> не передан, бот получит все обновления, произошедшие с момента последнего подтверждения.
        /// </remarks>
        /// <param name="limit">Максимальное количество получаемых обновлений.</param>
        /// <param name="timeout">Таймаут в секундах для длинного опроса.</param>
        /// <param name="marker">Если параметр передан, бот получит обновления, которые ещё не были получены. Если он не передан, он будет получать все новые обновления.</param>
        /// <param name="types">Список типов обновлений, которые бот хочет получать.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<UpdatesResponse> GetUpdatesAsync(int limit = 100, int timeout = 30, long? marker = null, string[]? types = null, CancellationToken cancellationToken = default)
        {
            if (limit < 1 || limit > 1000)
                throw new ArgumentOutOfRangeException(nameof(limit), string.Format(Error.RangeError, nameof(limit), 1, 1000));

            if (timeout < 0 || timeout > 90)
                throw new ArgumentOutOfRangeException(nameof(timeout), string.Format(Error.RangeError, nameof(timeout), 0, 90));

            var requestUri = new StringBuilder($"/updates?limit={limit}&timeout={timeout}");

            if (marker != null)
                requestUri.Append($"&marker={marker}");

            if (types != null)
                requestUri.Append($"&types={string.Join(',', types)}");

            requestUri.Append($"&access_token={accessToken}");

            return await HttpClientMax.GetAsync<UpdatesResponse>(requestUri.ToString(), cancellationToken);
        }

        [GeneratedRegex("^[a-zA-Z0-9_-]{5,256}$")]
        private static partial Regex SecretRegex();
    }
}
