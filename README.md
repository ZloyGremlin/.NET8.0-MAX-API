# NewMaxApi (.NET)

Это API для мессенджера **MAX** на **.NET 8.0**, созданное на основе официальной документации.  
Проект представляет собой .NET-библиотеку-обёртку над **MAX Platform API** (`https://platform-api.max.ru`) + пример бота на long-polling.

> ⚠️ В этой репозитории описана **клиентская библиотека**. Источником истины по полям/ответам остаётся официальная документация MAX.

## Возможности

- `Bots` — получить/изменить профиль бота (`/me`)
- `Chats` — управление чатами и участниками (`/chats/*`)
- `Messages` — отправка/получение/удаление сообщений, ответы на callback (`/messages`, `/answers`, `/videos/*`)
- `Subscriptions` — WebHook подписки и long-polling обновления (`/subscriptions`, `/updates`)
- `Uploads` — получение upload URL и загрузка файлов/картинок (`/uploads` + multipart upload)

## Требования

- .NET (SDK) 6+ (пример бота использует `Microsoft.Extensions.Hosting`)
- Access Token вашего бота (MAX)

## Установка

### Вариант 1 — готовая DLL из Releases (рекомендуется)
1. Откройте раздел **Releases** в этом репозитории и скачайте архив с библиотекой.
2. Распакуйте, например в папку `libs/NewMaxApi/`.
3. Подключите DLL к проекту:
   - **Visual Studio**: Project → Add → **Project Reference…** (если это проект) или **Add Reference… → Browse** (если DLL)
   - **CLI**: удобнее положить DLL в проект и подключить как reference (см. пример ниже)

Пример подключения DLL в `.csproj`:

```xml
<ItemGroup>
  <Reference Include="NewMaxApi">
    <HintPath>libs/NewMaxApi/NewMaxApi.dll</HintPath>
  </Reference>
</ItemGroup>

### Вариант 2 — собрать из исходников
Требования: .NET SDK 8.0+

```bash
dotnet restore
dotnet build -c Release

## Быстрый старт

```csharp
using NewMaxApi;

// 1) создаём провайдер
var api = new MaxApiProvider("YOUR_ACCESS_TOKEN");

// 2) читаем инфо о боте
var me = await api.Bots.GetBotInfoAsync();

// 3) отправляем сообщение в чат
await api.Messages.SendMessageAsync(
    new NewMaxApi.Requests.Messages.SendMessageRequest { /* text, attachments, ... */ },
    chatId: 123456789
);
```

## Пример long-polling бота

См. `Program.cs` + `MaxBotService.cs`.  
Идея: циклически вызываем `Subscriptions.GetUpdatesAsync(...)`, обрабатываем события и подтверждаем маркер следующего обновления.

## Документация по API (эндпоинты)

- [docs/api.md](docs/api.md)
- [docs/usage.md](docs/usage.md)

## Таймауты

- Для большинства вызовов достаточно 60–100 сек.
- Для long-polling (`/updates`) рекомендуется 120 сек и выше (в примере — 120).

## Лицензия

Apache-2.0
