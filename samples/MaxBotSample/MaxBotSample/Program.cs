using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewMaxApi;
using System.Net;

namespace TestBot
{
    public class Program
    {
        private const string MaxApiAccessToken = "тут ваш токен";

        public static async Task Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Регистрируем HttpClient для MAX API
                    services.AddHttpClient("MaxApi", client =>
                    {
                        client.BaseAddress = new Uri("https://platform-api.max.ru");

                        // Общий таймаут для большинства запросов.
                        // Для long-polling лучше 120 сек.
                        client.Timeout = TimeSpan.FromSeconds(120);
                    })
                    .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                    {
                        UseCookies = false,
                        AutomaticDecompression =
                            DecompressionMethods.GZip |
                            DecompressionMethods.Deflate |
                            DecompressionMethods.Brotli
                    });

                    // Регистрируем MaxApiProvider как Singleton
                    services.AddSingleton<MaxApiProvider>(sp =>
                    {
                        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                        var httpClient = httpClientFactory.CreateClient("MaxApi");

                        // requestTimeout можно не передавать, если уже задан в AddHttpClient.
                        // Но можно передать явно:
                        return new MaxApiProvider(
                            MaxApiAccessToken,
                            httpClient,
                            requestTimeout: TimeSpan.FromSeconds(120));
                    });

                    services.Configure<HostOptions>(o =>
                    {
                        o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
                    });

                    services.AddHostedService<MaxBotService>();

                    services.AddLogging(configure =>
                    {
                        configure.AddConsole();
                        configure.SetMinimumLevel(LogLevel.Information);
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}