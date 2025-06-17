# ShopSystem

## Описание

**ShopSystem** — это учебный микросервисный проект для управления заказами и счетами пользователей. Архитектура построена на взаимодействии через очередь сообщений и реализует надёжную доставку событий с использованием шаблонов Transactional Outbox/Inbox. В проекте применены современные подходы к разработке распределённых систем.

---

## Архитектура

### Компоненты

- **API Gateway**  
  Отвечает за маршрутизацию запросов:
  - `/orders/{*}` → Orders Service  
  - `/accounts/{*}` → Payments Service

- **Order Service (`Orders.Api`)**  
  Отвечает за создание и отслеживание заказов.
  - `Api`
  - `App`
  - `Domain`
  - `Infrastructure`

- **Payments Service (`Payments.Api`)**  
  Управляет пользовательскими счетами. Обрабатывает события создания заказа, проводит оплату и публикует результат.
  - `Api`
  - `App`
  - `Domain`
  - `Infrastructure`

- **RabbitMQ**  
  Используется как брокер сообщений. Задействованы очереди:
  - `orders.events`
  - `payments.events`

- **PostgreSQL**  
  Отдельные базы данных:
  - `orders`
  - `payments`  
  Миграции выполняются автоматически при запуске контейнеров.

---

## Функциональные возможности

### Orders Service

- `POST /orders` — создать заказ (и отправить событие в очередь)
- `GET /orders` — получить список заказов
- `GET /orders/{id}` — получить статус заказа

### Payments Service

- `POST /accounts` — создать счёт (один на пользователя)
- `POST /accounts/{id}/top-up` — пополнить счёт
- `GET /accounts/{id}` — получить текущий баланс

> Все контроллеры используют MediatR, работают с DTO и возвращают корректные HTTP-статусы.

---

## Архитектурные подходы

- **Transactional Outbox (Orders Service)**  
  Заказ и событие создаются в рамках одной транзакции в базе данных (EF Core).

- **Transactional Inbox (Payments Service)**  
  Обработка события `OrderCreated` происходит с сохранением входящего сообщения и фильтрацией по `EventId` (чтобы избежать повторной обработки).

- **Transactional Outbox (Payments Service)**  
  Результат проведения платежа (успех или ошибка) сохраняется и публикуется как отдельное событие.

- **Обработка at-most-once**  
  Повторная обработка исключается за счёт хранения `EventId` и проверки уникальности события.

---

## Swagger

### Orders Service
- [Swagger UI](http://localhost:8080/swagger/index.html)
- [OpenAPI JSON](http://localhost:8080/swagger/v1/swagger.json)

### Payments Service
- [Swagger UI](http://localhost:8081/swagger/index.html)
- [OpenAPI JSON](http://localhost:8081/swagger/v1/swagger.json)

---

## Запуск с Docker

### `docker-compose.yml` поднимает:

- RabbitMQ с UI на порту `15672`
- PostgreSQL для `orders` и `payments`, с `healthcheck`
- `orders-api` (порт `8080`)
- `payments-api` (порт `8081`)
- `gateway` (порт `8000`)

> Все сервисы имеют `health check` по эндпоинту `/healthz`.

### Команда запуска

```bash
docker-compose up --build -d
