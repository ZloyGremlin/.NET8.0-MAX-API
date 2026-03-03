# Usage / Recipes

## 1) Правильный HttpClient (без cookies + с декомпрессией)

В `MaxApiProvider` и `Program.cs` уже показаны настройки:
- `UseCookies = false`
- `AutomaticDecompression = GZip | Deflate | Brotli`
- таймаут 60–120 сек (long-polling лучше 120+)

## 2) Long-polling цикл

Типовой алгоритм:
1. `var updates = await api.Subscriptions.GetUpdatesAsync(limit:100, timeout:30, marker:lastMarker);`
2. обработать `updates`
3. сохранить `marker` из ответа как `lastMarker` и повторить

Важно:
- `timeout` в запросе ≤ 90 (ограничение на стороне API; см. код)
- таймаут HttpClient должен быть больше `timeout` (например 120 сек)

## 3) Отправка «печатает…»

```csharp
await api.Chats.SendActionAsync(chatId, SenderAction.Typing);
```

## 4) Загрузка картинки и отправка

Псевдокод:
```csharp
// 1) получить upload url
var upload = await api.Upload.GetUploadUrlAsync(UploadType.Image);

// 2) загрузить байты
var uploaded = await api.Upload.UploadImageAsync(upload.Url, bytes, "photo.jpg");

// 3) отправить сообщение с attachment (зависит от вашей модели SendMessageRequest)
await api.Messages.SendMessageAsync(new SendMessageRequest {
  // Text = "...",
  // Attachments = new [] { uploaded.AttachmentToken ... }
}, chatId: chatId);
```

## 5) WebHook

1) Подписаться:
```csharp
await api.Subscriptions.SubscribeAsync(new SubscribeRequest {
  Url = "https://your-host/path",
  Secret = "your_secret_123"
});
```

2) На стороне сервера:
- принимать POST от MAX
- проверять `secret` (если MAX его передаёт)
- отвечать быстро (200 OK)

## 6) Проверки и нюансы библиотеки

По коду видно пару мест, которые стоит перепроверить по оф. доке MAX:
- `Messages.EditMessageAsync(...)` принимает `messageId`, но URL не содержит `message_id`
- `Messages.GetVideoInfoAsync(...)` не добавляет `access_token` к `/videos/{token}`
- `Messages.SendAnswerAsync(...)` валидирует `callbackId`, но не использует его в URL (скорее всего он в body)

Если это баги — поправьте методы и обновите документацию.
