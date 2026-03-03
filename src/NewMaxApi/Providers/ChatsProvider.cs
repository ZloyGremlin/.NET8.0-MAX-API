using NewMaxApi.Entities;
using NewMaxApi.Providers;
using NewMaxApi.Requests.Chats;
using NewMaxApi.Resources;
using NewMaxApi.Responses;
using NewMaxApi.Responses.Chats;
using System.Net.Http;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

namespace NewMaxApi
{
    /// <summary>
    /// Предоставляет методы взаимодействия с конечными точками чата в API MAX.
    /// </summary>
    /// <param name="accessToken">Токен доступа, используемый для аутентификации API.</param>
    /// <param name="httpClient"> <see cref="HttpClient"/> экземпляр, используемый для запросов API.</param>
    public partial class ChatsProvider(string accessToken, HttpClient httpClient) : BaseProvider(httpClient)
    {
        protected override string PathTemplate => "/chats";

        /// <summary>
        /// Возвращает информацию о чатах, в которых участвовал бот. Результат включает список чатов и маркер перехода на следующую страницу.
        /// </summary>
        /// <param name="count">Количество запрошенных чатов.</param>
        /// <param name="marker">Указатель на следующую страницу данных. Для первой страницы передайте null.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<AllChatsResponse> GetAllChatsAsync(int count = 50, long? marker = null, CancellationToken cancellationToken = default)
        {
            if (count < 1 || count > 100)
                throw new ArgumentOutOfRangeException(nameof(count), string.Format(Error.RangeError, nameof(count), 1, 100));

            return await HttpClientMax.GetAsync<AllChatsResponse>($"{PathTemplate}?count={count}&marker={marker}&access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает информацию о чате по его публичной ссылке или информацию о беседе с пользователем по его имени пользователя.
        /// </summary>
        /// <param name="chatLink">Публичная ссылка на чат или имя пользователя.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatInfoResponse> GetChatAsync(string chatLink, CancellationToken cancellationToken = default)
        {
            if (!ChatLinkRegex().IsMatch(chatLink))
                throw new ArgumentException(Error.ChatLinkRegexError, nameof(chatLink));

            return await HttpClientMax.GetAsync<ChatInfoResponse>($"{PathTemplate}/{chatLink}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает информацию о чате по его идентификатору.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatInfoResponse> GetChatAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.GetAsync<ChatInfoResponse>($"{PathTemplate}/{chatId}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Позволяет редактировать информацию чата, включая имя, значок и закрепленное сообщение.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatInfoResponse> ChangeChatInfoAsync(long chatId, ChangeChatRequest body, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);

            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.PatchAsync<ChatInfoResponse>($"{PathTemplate}/{chatId}?access_token={accessToken}", body, cancellationToken);
        }

        /// <summary>
        /// Удаляет чат для всех участников.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> DeleteChatAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}/{chatId}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Позволяет отправлять в чат действия бота, такие как «набор текста» или «отправка фотографий».
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="action">Действие, отправленное участникам чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> SendActionAsync(long chatId, SenderAction action, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.PostAsync<SuccessResponse>($"{PathTemplate}/{chatId}/actions?access_token={accessToken}", action, cancellationToken);
        }

        /// <summary>
        /// Возвращает закрепленное сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<PinnedMessageResponse> GetPinnedMessageAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.GetAsync<PinnedMessageResponse>($"{PathTemplate}/{chatId}/pin?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Закрепите сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> PinMessageAsync(long chatId, PinMessageRequest body, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);

            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.PutAsync<SuccessResponse>($"{PathTemplate}/{chatId}/pin?access_token={accessToken}", body, cancellationToken);
        }

        /// <summary>
        /// Удаляет закрепленное сообщение в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> DeletePinnedMessageAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}/{chatId}/pin?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает информацию о текущем членстве бота в чате.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatMembershipResponse> GetChatMembershipAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return new ChatMembershipResponse
            {
                Membership = await HttpClientMax.GetAsync<ChatMember>($"{PathTemplate}/{chatId}/members/me?access_token={accessToken}", cancellationToken)
            };
        }

        /// <summary>
        /// Удаляет бота из участников чата.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> RemoveBotAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}/{chatId}/members/me?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает всех администраторов чата. Бот должен быть администратором в запрашиваемом чате.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatMembersResponse> GetAdminsAsync(long chatId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.GetAsync<ChatMembersResponse>($"{PathTemplate}/{chatId}/members/admins?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Назначает администратора чата. Возвращает значение <c>true</c>, если все администраторы добавлены.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="body">Тело запроса.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> AppointAdminAsync(long chatId, AppointAdminRequest body, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(body);

            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            return await HttpClientMax.PostAsync<SuccessResponse>($"{PathTemplate}/{chatId}/members/admins?access_token={accessToken}", body, cancellationToken);
        }

        /// <summary>
        /// Отменяет права администратора пользователя в чате, лишая его административных привилегий.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="userId">Идентификатор пользователя.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> RevokeAdminRightsAsync(long chatId, long userId, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException($"{nameof(chatId)} must match the regular expression \\-?\\d+", nameof(chatId));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            return await HttpClientMax.DeleteAsync<SuccessResponse>($"{PathTemplate}/{chatId}/members/admins/{userId}?access_token={accessToken}", cancellationToken);
        }

        /// <summary>
        /// Возвращает пользователей, участвующих в чате.
        /// </summary>
        /// <remarks>
        /// При передаче параметра <paramref name="userIds"/> параметры <paramref name="count"/> и <paramref name="marker"/> игнорируются.
        /// </remarks>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="userIds">Список идентификаторов пользователей, для которых необходимо получить членство. При передаче этого параметра параметры <paramref name="count"/> и <paramref name="marker"/> игнорируются.</param>
        /// <param name="marker">Указатель на следующую страницу данных.</param>
        /// <param name="count">Количество участников, которых необходимо вернуть.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<ChatMembersResponse> GetMembersAsync(long chatId, long[]? userIds = null, long? marker = null, int? count = null, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            if (count != null && (count < 1 || count > 100))
                throw new ArgumentOutOfRangeException(nameof(count), string.Format(Error.RangeError, nameof(count), 1, 100));

            var requestUri = new StringBuilder($"{PathTemplate}/{chatId}/members");

            if (userIds?.Length > 0)
            {
                requestUri.Append($"?user_ids={string.Join(",", userIds)}");
            }
            else
            {
                if (count != null && marker != null)
                {
                    requestUri.Append($"?count={count}&marker={marker}");
                }
                else if (count != null)
                {
                    requestUri.Append($"?count={count}");
                }
                else if (marker != null)
                {
                    requestUri.Append($"?marker={marker}");
                }
            }

            requestUri.Append($"&access_token={accessToken}");

            return await HttpClientMax.GetAsync<ChatMembersResponse>(requestUri.ToString(), cancellationToken);
        }

        /// <summary>
        /// Добавляет участников в чат. Для этого могут потребоваться дополнительные права.
        /// </summary>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="userIds">Идентификаторы пользователей.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> AddParticipantsAsync(long chatId, long[] userIds, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            if (userIds == null || userIds.Length == 0)
                throw new ArgumentException(string.Format(Error.ParameterNotSpecified, nameof(userIds)), nameof(userIds));

            return await HttpClientMax.PostAsync<SuccessResponse>($"{PathTemplate}/{chatId}/members?access_token={accessToken}", userIds, cancellationToken);
        }

        /// <summary>
        /// Удаляет участника из чата. Для этого могут потребоваться дополнительные права.
        /// </summary>
        /// <remarks>
        /// Если параметр <paramref name="block"/> установлен в значение <c>true</c>, пользователь будет заблокирован в чате. Используется только для чатов с публичной или приватной ссылкой. Во всех остальных случаях игнорируется.
        /// </remarks>
        /// <param name="chatId">Идентификатор запрошенного чата.</param>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="block">Если установлено в значение <c>true</c>, пользователь будет заблокирован в чате. Используется только для чатов с публичной или приватной ссылкой. Во всех остальных случаях игнорируется.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<SuccessResponse> DeleteParticipantAsync(long chatId, long userId, bool block = false, CancellationToken cancellationToken = default)
        {
            if (!ChatIdRegex().IsMatch(chatId.ToString()))
                throw new ArgumentException(Error.ChatIdRegexError, nameof(chatId));

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId);

            var requestUri = new StringBuilder($"{PathTemplate}/{chatId}/members?user_id={userId}");

            if (block)
                requestUri.Append($"&block={block}");

            requestUri.Append($"&access_token={accessToken}");

            return await HttpClientMax.DeleteAsync<SuccessResponse>(requestUri.ToString(), cancellationToken);
        }

        [GeneratedRegex("@?[a-zA-Z]+[a-zA-Z0-9-_]*")]
        private static partial Regex ChatLinkRegex();

        [GeneratedRegex("\\-?\\d+")]
        private static partial Regex ChatIdRegex();
    }
}