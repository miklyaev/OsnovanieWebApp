# OsnovanieWebApp

Пет-проект для обучения современным технологиям .NET: веб-API, gRPC, очередям сообщений (Kafka, RabbitMQ), кэшированию (Redis), базам данных (PostgreSQL, ClickHouse) и real-time коммуникации (SignalR).

---

## Описание решения

Решение объединяет несколько сервисов и консольных приложений, которые демонстрируют типичные сценарии enterprise-приложений: REST API, RPC через gRPC, асинхронная обработка сообщений, кэширование, хранение данных и доставка событий в реальном времени.

**Ключевые технологии:** ASP.NET Core, gRPC, Kafka, RabbitMQ, Redis, PostgreSQL, Entity Framework Core, ClickHouse, SignalR, Serilog, Docker.

---

## Проекты решения

### OsnovanieWebApp

**Тип:** ASP.NET Core Web API (net9.0)

**Назначение:** Точка входа для клиентов. Предоставляет REST API для работы с пользователями, книгами, авторами, регионами, запросами и сигналами; часть операций проксируется к gRPC-сервису и Kafka.

**Технологии:** ASP.NET Core, AutoWrapper (единый формат ответов), Swagger/OpenAPI, Serilog (консоль, файл, Seq), StackExchange Redis (распределённый кэш).

**Основные контроллеры и маршруты:**
- `UserController` — getUser, getAll, addUser (данные через gRPC, кэш Redis).
- `BookController` — addBook, addAuthor, updateBook (через gRPC).
- `RegionController` — addRegion (через gRPC).
- `KafkaController` — addMessage (запись в Kafka), readMessage (чтение из топика).
- `SignalRController` — addSignal (отправка сигнала в Kafka для последующей доставки через RabbitMQ/SignalR).
- `RequestController` — view, list (демо-запросы по JSON).
- `HelloWorldController`, `WeatherForecastController` — примеры эндпоинтов.

**Связи:** Зависит от **OsnovanieService**. Использует Redis для кэширования ответов gRPC (пользователи). Все вызовы к данным и Kafka идут через `IMainService` → gRPC-клиент к **GrpcService1**.

---

### OsnovanieService

**Тип:** Библиотека класса (net9.0)

**Назначение:** Бизнес-слой и gRPC-клиент. Реализует `IMainService`: обращается к **GrpcService1** по gRPC, кэширует результаты в Redis (IDistributedCache), читает/пишет в Kafka через gRPC-сервис.

**Технологии:** Grpc.Net.Client, Microsoft.Extensions.Caching.Abstractions (Redis используется на стороне WebApp), Serilog, Newtonsoft.Json.

**Связи:** Содержит Protobuf-клиент (greet.proto). Используется только проектом **OsnovanieWebApp**.

---

### GrpcService1

**Тип:** ASP.NET Core приложение с gRPC-сервером (net9.0)

**Назначение:** gRPC-сервис и бэкенд данных. Обрабатывает запросы по пользователям, авторам, книгам, регионам, сигналам; пишет в PostgreSQL (EF Core) и в Kafka.

**Технологии:** Grpc.AspNetCore, Entity Framework Core, Npgsql (PostgreSQL), KafkaLibNetCore (Confluent.Kafka), Serilog, Docker.

**Модели и операции:** Пользователи (User), авторы (Author), книги (Book), регионы (Region), сигналы (Signal). Часть операций дублируется в Kafka (например, AddUserToKafka, ReadFromKafka, AddSignalToKafka).

**Связи:** Зависит от **KafkaLibNetCore**. Использует `ApplicationContext` (EF Core) и `INpgSqlService` для PostgreSQL, `ICustomProducer`/`ICustomConsumer` для Kafka. Вызывается по gRPC из **OsnovanieWebApp** (через OsnovanieService).

---

### KafkaLibNetCore

**Тип:** Библиотека класса (net9.0), может собираться в NuGet-пакет

**Назначение:** Обёртка над Confluent.Kafka для producer и consumer с поддержкой DI и конфигурации.

**Технологии:** Confluent.Kafka, Microsoft.Extensions.Configuration, Serilog.

**API:** `ICustomProducer` (WriteToKafka, WriteToKafkaAsync, ConfigProducer), `ICustomConsumer<TKey, TValue>` (ConfigConsumer, SubscribeTopic/SubscribeTopics, ReadFromKafka, Commit, ConsumerClose).

**Связи:** Используется **GrpcService1** и **KafkaToRabbitMq** / **KafkaToRabbitMqConsole** для работы с Kafka.

---

### KafkaToRabbitMq

**Тип:** Worker-сервис (BackgroundService, net9.0)

**Назначение:** Мост Kafka → RabbitMQ. Читает сообщения из топика Kafka и публикует их в RabbitMQ; может работать как Windows-служба.

**Технологии:** KafkaLibNetCore (ссылка на пакет), RabbitMQ.Client, Microsoft.Extensions.Hosting, Serilog.

**Связи:** Потребляет **KafkaLibNetCore**. Конфигурация Kafka и RabbitMQ через appsettings/переменные окружения. Сообщения из Kafka пересылаются в очередь RabbitMQ для подписчиков (например, **SignalRApp**, **RabbitMqConsumer**).

---

### KafkaToRabbitMqConsole

**Тип:** Консольное приложение с хостингом (Worker-логика, net9.0)

**Назначение:** Упрощённый вариант моста Kafka → RabbitMQ для запуска из консоли; логика аналогична **KafkaToRabbitMq**.

**Технологии:** Ссылка на KafkaLibNetCore (DLL), Serilog, Microsoft.Extensions.Hosting.

**Связи:** Использует **KafkaLibNetCore** и RabbitMQ; отдельное решение KafkaToRabbitMqConsole.sln.

---

### SignalRApp

**Тип:** ASP.NET Core приложение с SignalR (net9.0)

**Назначение:** Real-time уведомления. Host для SignalR Hub (ChatHub) и фоновый сервис, подписанный на RabbitMQ; при получении сообщения из очереди отправляет его клиентам по SignalR.

**Технологии:** Microsoft.AspNetCore.SignalR, RabbitMQ.Client, System.Reactive (Rx), Serilog, Windows Services support.

**Связи:** Подписывается на RabbitMQ (консьюмер). Клиенты подключаются к Hub для получения сообщений в реальном времени. Цепочка: Kafka → **KafkaToRabbitMq** → RabbitMQ → **SignalRApp** → браузер/клиент.

---

### RabbitMqConsumer

**Тип:** Консольное приложение (.NET Framework 4.8)

**Назначение:** Простой консьюмер очереди RabbitMQ для приёма сообщений из брокера (демонстрация подписчика вне ASP.NET).

**Технологии:** RabbitMQ.Client, .NET Framework 4.8.

**Связи:** Входит в решение RabbitmqConsole.sln; подписывается на те же очереди, что и **SignalRApp**, при необходимости.

---

### ReactiveAppConsole

**Тип:** Консольное приложение (net9.0)

**Назначение:** Демонстрация реактивного потребления RabbitMQ с использованием System.Reactive (Rx).

**Технологии:** RabbitMQ.Client, System.Reactive (Rx), Microsoft.Extensions.Hosting, Serilog.

**Связи:** Отдельный консьюмер RabbitMQ; показывает паттерн реактивных потоков поверх очереди.

---

### ClickHouseApp

**Тип:** Worker-сервис (BackgroundService, net9.0)

**Назначение:** Запись данных в ClickHouse (аналитическая БД): пользователи и сигналы (телеметрия). Использует RestSharp к HTTP-интерфейсу ClickHouse и Polly для retry.

**Технологии:** Octonica.ClickHouseClient, RestSharp, Polly, Quartz (планировщик), Serilog, Docker.

**Связи:** Независимый сервис; получает данные из конфигурации/кода (в текущей реализации Worker сам создаёт тестового пользователя). Может расширяться приёмом сообщений из Kafka/RabbitMQ для записи в ClickHouse.

---

### docker-compose

**Тип:** Docker Compose-проект (dcproj)

**Назначение:** Сборка и запуск образа **OsnovanieWebApp** в контейнере.

**Связи:** Собирает Dockerfile из папки OsnovanieWebApp; в docker-compose.yml описан один сервис `osnovaniewebapp`.

---

## Ключевые связи между проектами

```
                    ┌─────────────────────┐
                    │   OsnovanieWebApp   │  REST API, Redis cache
                    │   (ASP.NET Core)    │
                    └──────────┬─────────┘
                               │ IMainService
                               ▼
                    ┌─────────────────────┐
                    │  OsnovanieService   │  gRPC client, business logic
                    └──────────┬─────────┘
                               │ gRPC
                               ▼
                    ┌─────────────────────┐     ┌──────────────────┐
                    │    GrpcService1     │────▶│  KafkaLibNetCore  │
                    │  (gRPC + EF + Kafka)│     └────────┬─────────┘
                    └──────────┬─────────┘              │
                               │                        ▼
                    PostgreSQL (Npgsql)           Apache Kafka
                                                         │
                    ┌─────────────────────┐              │
                    │   KafkaToRabbitMq   │◀─────────────┘
                    │  (Worker: K→R bridge)│
                    └──────────┬──────────┘
                               │
                               ▼
                         RabbitMQ
                               │
           ┌───────────────────┼───────────────────┐
           ▼                   ▼                     ▼
  ┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
  │   SignalRApp    │ │ RabbitMqConsumer│ │ ReactiveApp     │
  │ (SignalR Hub +  │ │ (.NET FX console)│ │ Console (Rx)   │
  │  RabbitMQ sub)  │ │                 │ │                 │
  └─────────────────┘ └─────────────────┘ └─────────────────┘

  ┌─────────────────┐
  │  ClickHouseApp  │  Worker → ClickHouse (RestSharp/HTTP)
  └─────────────────┘
```

- **Поток данных для пользователей/книг:** браузер/клиент → OsnovanieWebApp (REST) → OsnovanieService → GrpcService1 (gRPC) → PostgreSQL; кэш в Redis в WebApp.
- **Поток для Kafka:** клиент → OsnovanieWebApp (например, addMessage/addSignal) → OsnovanieService → GrpcService1 → Kafka; при необходимости KafkaToRabbitMq пересылает в RabbitMQ → SignalRApp → клиенты по SignalR.

---

## Что нужно сделать

### Тестирование

1. **Unit-тесты**
   - **OsnovanieService:** тесты для `MainService` с моками gRPC-клиента и `IDistributedCache` (xUnit/NUnit + Moq/NSubstitute).
   - **GrpcService1:** тесты для `GreeterService` и `NpgSqlService` с in-memory EF или тестовой БД.
   - **KafkaLibNetCore:** тесты для `Consumer`/`Producer` с моками или embedded Kafka (например, Testcontainers).

2. **Интеграционные тесты**
   - **OsnovanieWebApp:** `WebApplicationFactory` для проверки контроллеров (User, Book, Kafka, SignalR), с подменой gRPC и Redis при необходимости.
   - **GrpcService1:** вызов gRPC-методов через тестовый хост и реальную тестовую PostgreSQL (или Testcontainers).
   - **KafkaToRabbitMq:** интеграционный сценарий Kafka → RabbitMQ с тестовыми брокерами (Testcontainers).

3. **E2E (по желанию)**
   - Сценарий: REST → gRPC → БД и REST → Kafka → RabbitMQ → SignalR с поднятием минимального набора сервисов (например, через docker-compose для тестов).

4. **Структура**
   - Добавить в решение тестовые проекты: `OsnovanieWebApp.Tests`, `OsnovanieService.Tests`, `GrpcService1.Tests`, `KafkaLibNetCore.Tests` и при необходимости интеграционные проекты с общим запуском инфраструктуры.

---

### Где добавить OAuth 2.0

1. **OsnovanieWebApp (REST API)**
   - Основной кандидат: защита всех API эндпоинтов с помощью JWT Bearer, выдаваемого по OAuth 2.0 (Authorization Code или Resource Owner Password — только для обучения).
   - Шаги: добавить пакет `Microsoft.AspNetCore.Authentication.JwtBearer`, настроить `AddAuthentication`/`AddJwtBearer`, использовать `[Authorize]` на контроллерах или глобально; при необходимости отдельный проект или конфиг для IdentityServer/Duende, Keycloak или Azure AD как Authorization Server.

2. **GrpcService1 (gRPC)**
   - Передача JWT в метаданных gRPC и валидация на сервере (Bearer token из заголовка или `Authorization` в gRPC metadata).
   - Настроить `JwtBearer` в gRPC-хосте и проверять `HttpContext.User` в сервисах или через middleware/interceptor.

3. **SignalRApp**
   - Подключение к Hub только для аутентифицированных пользователей: использовать тот же JWT (QueryString или header), что и в API, и настроить `[Authorize]` на `ChatHub`.

4. **Единый вход (SSO) для обучения**
   - Вынести OAuth 2.0 в один провайдер (например, отдельное приложение IdentityServer/Duende или Keycloak в Docker), чтобы и WebApp, и gRPC, и SignalR запрашивали токены у одного Authorization Server и проверяли подпись/issuer по единым настройкам.

Итог: логичнее всего начать с **OsnovanieWebApp** (OAuth 2.0 + JWT для REST), затем распространить тот же токен на **GrpcService1** и **SignalRApp**, при необходимости введя отдельный проект Identity Provider.
