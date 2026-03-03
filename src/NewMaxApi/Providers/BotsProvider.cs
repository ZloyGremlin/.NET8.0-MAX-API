using NewMaxApi.Requests.Bots;
using NewMaxApi.Entities;
using NewMaxApi.Responses.Bots;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace NewMaxApi.Providers
{
    /// <summary>
    /// Предоставляет методы взаимодействия с конечными точками, связанными с ботами, в API MAX.
    /// </summary>
    /// <param name="accessToken">Токен доступа, используемый для аутентификации API.</param>
    /// <param name="httpClient"> <see cref="HttpClient"/> экземпляр, используемый для запросов API</param>
    public class BotsProvider(string accessToken, HttpClient httpClient) : BaseProvider(httpClient)
    {
        protected override string PathTemplate => "/me";

        /// <summary>
        /// Возвращает информацию о текущем боте, идентифицированном с помощью токена доступа. Метод возвращает идентификатор бота, его имя и аватар (если есть).
        /// </summary>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<BotInfoResponse> GetBotInfoAsync(CancellationToken cancellationToken = default)
        {
            return await HttpClientMax.GetAsync<BotInfoResponse>($"{PathTemplate}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Позволяет изменить информацию о текущем боте. Заполните только те поля, которые необходимо обновить. Все остальные останутся без изменений.
        /// </summary>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<BotInfoResponse> ChangeBotInfoAsync(ChangeBotRequest body, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);
            return await HttpClientMax.PatchAsync<BotInfoResponse>($"{PathTemplate}?access_token={accessToken}", body, cancellationToken);
        }
    }
}