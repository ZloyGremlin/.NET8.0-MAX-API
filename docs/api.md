# API reference (по коду библиотеки)

Базовый URL: `https://platform-api.max.ru`  
Авторизация: библиотека добавляет `access_token` в query string (как в коде провайдеров).

> Ниже — описание *маршрутов*, которые использует библиотека.  
> Схемы JSON (поля request/response) зависят от классов `NewMaxApi.Requests.*` и `NewMaxApi.Responses.*` в вашем проекте.

---

## Bots

### GET `/me?access_token=...`
Возвращает информацию о текущем боте.

**C#**
```csharp
var me = await api.Bots.GetBotInfoAsync();
```

### PATCH `/me?access_token=...`
Изменяет информацию о текущем боте.

**Body:** `ChangeBotRequest`  
**C#**
```csharp
await api.Bots.ChangeBotInfoAsync(new ChangeBotRequest {
  // заполняйте только нужные поля
});
```

---

## Chats

### GET `/chats?count={1..100}&marker={marker?}&access_token=...`
Список чатов, где участвовал бот. Ответ содержит список чатов и `marker` следующей страницы.

**C#**
```csharp
var page = await api.Chats.GetAllChatsAsync(count: 50, marker: null);
```

### GET `/chats/{chatLink}?access_token=...`
Информация о чате по публичной ссылке или диалог по username.

`chatLink` должен соответствовать regex: `@?[a-zA-Z]+[a-zA-Z0-9-_]*`

### GET `/chats/{chatId}?access_token=...`
Информация о чате по ID (`chatId` regex: `\-?\d+`)

### PATCH `/chats/{chatId}?access_token=...`
Редактирование информации чата.

**Body:** `ChangeChatRequest`

### DELETE `/chats/{chatId}?access_token=...`
Удалить чат для всех участников.

### POST `/chats/{chatId}/actions?access_token=...`
Отправить действие (например, «печатает…», «отправляет фото»).

**Body:** `SenderAction` (enum/entity)

### GET `/chats/{chatId}/pin?access_token=...`
Получить закреплённое сообщение.

### PUT `/chats/{chatId}/pin?access_token=...`
Закрепить сообщение.

**Body:** `PinMessageRequest`

### DELETE `/chats/{chatId}/pin?access_token=...`
Удалить закреп.

### GET `/chats/{chatId}/members/me?access_token=...`
Получить членство бота в чате.

### DELETE `/chats/{chatId}/members/me?access_token=...`
Удалить бота из чата.

### GET `/chats/{chatId}/members/admins?access_token=...`
Список администраторов чата.

### POST `/chats/{chatId}/members/admins?access_token=...`
Назначить админов.

**Body:** `AppointAdminRequest`

### DELETE `/chats/{chatId}/members/admins/{userId}?access_token=...`
Отозвать права администратора.

### GET `/chats/{chatId}/members?...&access_token=...`
Получить участников чата.

Варианты query:
- `user_ids=1,2,3` — получить членство конкретных пользователей (в этом случае `count`/`marker` игнорируются)
- `count={1..100}`
- `marker={marker}`

Пример:
`/chats/123/members?count=50&marker=999&access_token=...`

### POST `/chats/{chatId}/members?access_token=...`
Добавить участников в чат.

**Body:** массив `long[]` (`userIds`)

### DELETE `/chats/{chatId}/members?user_id={userId}&block={true|false}&access_token=...`
Удалить участника из чата.  
Если `block=true`, пользователь будет заблокирован (для чатов с публичной/приватной ссылкой).

---

## Messages

### GET `/messages?chat_id={chatId}&...&access_token=...`
Получить сообщения чата.

Query-параметры:
- `chat_id` (обязательный)
- `message_ids=mid1,mid2,...` (опционально)
- `from` / `to` (unix timestamp, опционально)
- `count` (1..100, опционально)

> Сообщения приходят в обратном порядке (последние — первыми).  
> Если используете `from` и `to`, то `to` должно быть меньше `from` (см. remarks в коде).

### POST `/messages?access_token=...&user_id={userId?}&chat_id={chatId?}&disable_link_preview={bool?}`
Отправить сообщение.

**Body:** `SendMessageRequest`

### PUT `/messages?access_token=...`
Редактировать сообщение.

**Внимание:** метод библиотеки принимает `messageId`, но в URL он не добавляется (см. `EditMessageAsync`).  
На практике ID редактируемого сообщения обычно передаётся в теле запроса — проверьте вашу модель `SendMessageRequest` и/или требования MAX.

### DELETE `/messages?message_id={messageId}&access_token=...`
Удалить сообщение.

### GET `/messages/{messageId}?access_token=...`
Получить одно сообщение.

### GET `/videos/{videoToken}`
Получить подробную информацию о видео.

> В коде `access_token` не добавляется к `/videos/{videoToken}`.  
> Если MAX требует токен и тут — добавьте `?access_token=...` в библиотеке.

### POST `/answers?access_token=...`
Ответить на callback кнопки.

**Body:** `SendAnswerRequest`  
`callbackId` валидируется, но **не добавляется** в URL. Обычно он находится в body — проверьте `SendAnswerRequest`.

---

## Subscriptions & Updates

### GET `/subscriptions?access_token=...`
Список WebHook подписок.

### POST `/subscriptions?access_token=...`
Создать WebHook подписку.

**Body:** `SubscribeRequest`  
Ограничение `secret`: regex `^[a-zA-Z0-9_-]{5,256}$`  
Порты для WebHook сервера (из remarks): `80, 8080, 443, 8443, 16384-32383`.

### DELETE `/subscriptions?url={url}&access_token=...`
Удалить подписку по URL.

### GET `/updates?limit={1..1000}&timeout={0..90}&marker={marker?}&types=t1,t2&access_token=...`
Long-polling обновления, если WebHook не используется.

- `limit`: до 1000
- `timeout`: до 90 секунд (сервер держит соединение)
- `marker`: подтверждённый маркер
- `types`: фильтр типов апдейтов

---



### Events / Update types (по примеру `MaxBotService`)

Эндпоинт `/updates` возвращает массив событий (updates). В примере бота они десериализуются в классы из `NewMaxApi.Entities` и обрабатываются через `switch` по runtime-типу.

> В параметре `types` в `/updates` MAX ожидает **строковые типы событий**.  
> Их точные значения (строки) лучше сверять с официальной докой MAX.  
> Ниже перечислены **типы, которые уже поддержаны примером бота** (по именам классов в коде).

#### Message events

- **`MessageCreated`** — пришло новое сообщение в чат  
  Полезные поля (как использует пример):
  - `u.Message.Recipient.ChatId` — ID чата
  - `u.Message.Body.Text` — текст (может быть `null`)
  - `u.Message.Body.Attachments` — вложения

- **`MessageEdited`** — сообщение отредактировано  
  - `u.Message.Recipient.ChatId`

- **`MessageRemoved`** — сообщение удалено  
  - `u.ChatId`

- **`MessageCallback`** — нажата кнопка / callback от inline keyboard  
  - `u.Message.Recipient.ChatId`
  - `u.Callback.Payload` — payload кнопки (пример обрабатывает `"ping"`)

#### Bot lifecycle

- **`BotStarted`** — бот «стартовал» в чате (например, добавили/активировали)  
  - `u.ChatId`
  - `u.Payload` — payload, если MAX его передал

- **`BotAdded`** — бота добавили в чат  
  - `u.ChatId`

- **`BotRemoved`** — бота удалили из чата  
  - `u.ChatId`

- **`BotStopped`** — бот остановлен/деактивирован в чате  
  - `u.ChatId`

#### Dialog events

- **`DialogMuted`** — диалог заглушён (mute)  
  - `u.ChatId`

- **`DialogUnmuted`** — диалог раззаглушён (unmute)  
  - `u.ChatId`

- **`DialogCleared`** — диалог очищен (история)  
  - `u.ChatId`

- **`DialogRemoved`** — диалог удалён  
  - `u.ChatId`

#### Membership / Chat meta

- **`UserAdded`** — пользователя добавили в чат  
  - `u.ChatId`
  - `u.User.UserId`

- **`UserRemoved`** — пользователя удалили из чата  
  - `u.ChatId`
  - `u.User.UserId`

- **`ChatTitleChanged`** — изменился заголовок (title) чата  
  - `u.ChatId`
  - `u.Title`

- **`MessageChatCreated`** — создан чат (событие уровня «чат создан»)  
  - `u.Chat.ChatId`

#### Фильтрация типов через `types`

Если хочешь получать только часть событий, передавай `types` в `GetUpdatesAsync(...)`:

```csharp
var resp = await api.Subscriptions.GetUpdatesAsync(
    limit: 100,
    timeout: 30,
    marker: lastMarker,
    types: new[]
    {
        // значения строк лучше брать из оф.доки MAX
        // например: "message_created", "message_callback", ...
    }
);
```


## Uploads

Загрузка файлов идёт в 2 шага:
1) получить upload URL
2) загрузить файл multipart/form-data

### POST `/uploads?access_token=...&type={type}`
Получить upload URL для указанного типа.

`type` берётся из enum `UploadType` и приводится к lower-case.

### POST `{uploadUrl}&access_token=...` (multipart/form-data)
Загрузить файл.

В multipart поле: `data` (файл), `filename` — имя файла.

В библиотеке есть 2 метода:
- `UploadFileAsync(...)` → `UploadFileResponse`
- `UploadImageAsync(...)` → `UploadImageResponse`

---

## Ошибки и обработка

Библиотека делает базовую валидацию входных параметров (regex/range).  
HTTP-ошибки и формат ошибок зависят от реализации `HttpClientHelper`.

Рекомендуется:
- логировать статус/тело при неуспехе
- оборачивать вызовы в retry (особенно long-polling) с backoff
