# Инструкция по запуску

## Требования

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [dotnet-ef](https://www.nuget.org/packages/dotnet-ef) (инструмент для миграций)

## 1. Запустить PostgreSQL через Docker

```bash
cd docker
docker compose up -d
```

Дождаться статуса `healthy`:

```bash
docker ps
```

## 2. Установить инструмент миграций (если не установлен)

```bash
dotnet tool install --global dotnet-ef
```

## 3. Применить миграции

```bash
cd task
dotnet ef database update
```

## 4. Запустить приложение

```bash
dotnet run
```

При старте приложение автоматически:
- Импортирует все терминалы из `files/terminals.json` в PostgreSQL
- Выведет в консоль структурированные логи:

```
info: Начало импорта терминалов
info: Загружено XXXX терминалов из JSON
info: Удалено 0 старых записей
info: Сохранено XXXX новых терминалов
info: Следующий импорт запланирован через 01:xx:xx
```

## 5. Проверить API

Swagger UI доступен по адресу: **http://localhost:5000** (или порт из консоли)