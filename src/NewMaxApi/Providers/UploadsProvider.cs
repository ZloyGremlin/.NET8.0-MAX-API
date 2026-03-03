using NewMaxApi.Entities;
using NewMaxApi.Enums;
using NewMaxApi.Resources;
using NewMaxApi.Responses.Uploads;
using NewMaxApi.Utilities;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace NewMaxApi.Providers
{

    /// <summary>
    /// Предоставляет методы для взаимодействия с конечными точками, связанными с загрузкой, в API MAX.
    /// </summary>
    /// <param name="accessToken">Токен доступа, используемый для аутентификации API.</param>
    /// <param name="httpClient">Экземпляр <see cref="HttpClient"/>, используемый для запросов API.</param>
    public class UploadsProvider
    {
        private HttpClient _httpClient;

        private string _token; 
        public UploadsProvider(string accessToken, HttpClient httpClient)
        {
            _token = accessToken;
            _httpClient = httpClient;
        }

        protected string PathTemplate => "/uploads";

        /// <summary>
        /// Запрашивает URL-адрес для загрузки файла определённого типа.
        /// </summary>
        /// <param name="type">Тип загружаемого файла.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<UploadResponse> GetUploadUrlAsync(UploadType type, CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{PathTemplate}?access_token={_token}&type={type.ToString().ToLower()}")
            {              
            };
            HttpClientHelper HttpClientMax = new HttpClientHelper(_httpClient);
            return await HttpClientMax.SendAsync<UploadResponse>(request, cancellationToken);
        }

        /// <summary>
        /// Загружает файл по указанному URL-адресу загрузки.
        /// </summary>
        /// <param name="uploadUrl">URL-адрес, полученный из <see cref="GetUploadUrlAsync"/> для загрузки файла.</param>
        /// <param name="filePath">Локальный путь к загружаемому файлу.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<UploadFileResponse> UploadFileAsync(string uploadUrl, byte[] fileBytes, string filename, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(uploadUrl))
                throw new ArgumentNullException(nameof(uploadUrl), string.Format(Error.ParameterNotSpecified, nameof(uploadUrl)));

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename), string.Format(Error.ParameterNotSpecified, nameof(filename)));
            //UploadFileResponse
            using (var fileStream = new MemoryStream(fileBytes))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{uploadUrl}&access_token={_token}")
                {
                };

                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                // Добавляем заголовок Content-Disposition с именем файла
                fileContent.Headers.Add("Content-Disposition", $"form-data; name=\"data\"; filename=\"{filename}\"");

                // Указываем тип содержимого как "application/octet-stream"
                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                content.Add(fileContent, "data", filename);
                request.Content = content;
                HttpClientHelper HttpClientMax = new HttpClientHelper(_httpClient);
                return await HttpClientMax.SendAsync<UploadFileResponse>(request, cancellationToken);
            }
        
        }

        /// <summary>
        /// Загружает фото по указанному URL-адресу загрузки.
        /// </summary>
        /// <param name="uploadUrl">URL-адрес, полученный из <see cref="GetUploadUrlAsync"/> для загрузки файла.</param>
        /// <param name="filePath">Локальный путь к загружаемому файлу.</param>
        /// <param name="cancellationToken">Токен отмены, который можно использовать для отмены операции.</param>
        public async Task<UploadImageResponse> UploadImageAsync(string uploadUrl, byte[] fileBytes, string filename, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(uploadUrl))
                throw new ArgumentNullException(nameof(uploadUrl), string.Format(Error.ParameterNotSpecified, nameof(uploadUrl)));

            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException(nameof(filename), string.Format(Error.ParameterNotSpecified, nameof(filename)));
            //UploadFileResponse
            using (var fileStream = new MemoryStream(fileBytes))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{uploadUrl}&access_token={_token}")
                {
                };

                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                // Добавляем заголовок Content-Disposition с именем файла
                fileContent.Headers.Add("Content-Disposition", $"form-data; name=\"data\"; filename=\"{filename}\"");

                // Указываем тип содержимого как "application/octet-stream"
                fileContent.Headers.Add("Content-Type", "application/octet-stream");
                content.Add(fileContent, "data", filename);
                request.Content = content;
                HttpClientHelper HttpClientMax = new HttpClientHelper(_httpClient);
                return await HttpClientMax.SendAsync<UploadImageResponse>(request, cancellationToken);
            }

        }
    }
}
