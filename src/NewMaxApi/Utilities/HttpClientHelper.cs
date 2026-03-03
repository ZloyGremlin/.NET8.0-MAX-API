using NewMaxApi.Exceptions;
using NewMaxApi.Resources;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Threading;

namespace NewMaxApi.Utilities
{
    public class HttpClientHelper(HttpClient httpClient)
    {
        private async static Task<T> GetResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken)
        {
            if (!response.IsSuccessStatusCode)
                throw new MaxApiException(string.Format(Error.SendingRequestError, response.ReasonPhrase, response.StatusCode));

            return await response.Content.ReadFromJsonAsync<T>(cancellationToken).ConfigureAwait(false);
        }

        public async Task<T> SendAsync<T>(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                var str1 = response.Content.ReadAsStringAsync();
            }
            var str2 = response.Content.ReadAsStringAsync();
            return await GetResponseAsync<T>(response, cancellationToken);
        }

        public async Task<T> GetAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(requestUri, cancellationToken).ConfigureAwait(false);
            return await GetResponseAsync<T>(response, cancellationToken);
        }

        public async Task<T> PostAsync<T>(string requestUri, object body, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync(requestUri, body, cancellationToken).ConfigureAwait(false);
            return await GetResponseAsync<T>(response, cancellationToken);
        }

        public async Task PostAsync(string requestUri, object body, CancellationToken cancellationToken)
        {
            var response = await httpClient.PostAsJsonAsync(requestUri, body, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                throw new MaxApiException(string.Format(Error.SendingRequestError, response.ReasonPhrase, response.StatusCode));
        }

        public async Task<T> PutAsync<T>(string requestUri, object body, CancellationToken cancellationToken)
        {
            var response = await httpClient.PutAsJsonAsync(requestUri, body, cancellationToken).ConfigureAwait(false);
            return await GetResponseAsync<T>(response, cancellationToken);
        }

        public async Task<T> PatchAsync<T>(string requestUri, object body, CancellationToken cancellationToken)
        {
            var response = await httpClient.PatchAsJsonAsync(requestUri, body, cancellationToken).ConfigureAwait(false);
            return await GetResponseAsync<T>(response, cancellationToken);
        }

        public async Task<T> DeleteAsync<T>(string requestUri, CancellationToken cancellationToken)
        {
            var response = await httpClient.DeleteAsync(requestUri, cancellationToken).ConfigureAwait(false);
            return await GetResponseAsync<T>(response, cancellationToken);
        }
    }
}
