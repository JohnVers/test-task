# ТЗ

Реализовать справочник терминалов.

## Задача

1. Создать фоновую службу (BackgroundService) для периодической загрузки справочника терминалов из JSON файла и сохранения данных в PostgreSQL.
2. Обеспечить доступ к справочнику через ApiController.
3. Создать методы:
"Поиск терминалов города по названию города и области" - возвращает список офисов найденных в городе.
"Поиск идентификатора города по названию города и области" - возвращает идентификатор города из найденной сущности Office.

### Технический стек

- Язык: C# 13 (.NET 9)
- Платформа: ASP.NET Core 9 (Hosted Service, без Web API)
- ORM: Entity Framework Core 9 (PostgreSQL)
- DI: Microsoft.Extensions.DependencyInjection
- Логирование: ILogger (структурированные логи)

## Источник данных

Файл:  ~/files/terminals.json
Расположение: Корневая папка приложения /files/
Формат: JSON массив объектов терминалов

### Сущности и DbContext

DbContext

```csharp
public class DellinDictionaryDbContext : DbContext
{
    public DbSet<Office> Offices { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
```

Office (терминал)

```csharp
public class Office
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public int CityCode { get; set; }
    public string? Uuid { get; set; }
    public OfficeType? Type { get; set; }
    public string CountryCode { get; set; }
    public Coordinates Coordinates { get; set; }
    public string? AddressRegion { get; set; }
    public string? AddressCity { get; set; }
    public string? AddressStreet { get; set; }
    public string? AddressHouseNumber { get; set; }
    public int? AddressApartment { get; set; }
    public string WorkTime { get; set; }
    public Phone Phones { get; set; }
    public Office() { }
}
```

```csharp
public class Phone
{
    public int Id { get; set; }

    public int OfficeId { get; set; }

    public string PhoneNumber { get; set; }

    public string? Additional { get; set; }

    public Office Office { get; set; }
}
```

```csharp
public class Coordinates
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
```

```csharp
public enum OfficeType
{
    PVZ,
    POSTAMAT,
    WAREHOUSE
}
```

### Функциональные требования

- Периодичность - ежедневно в 02:00 MSK
- Загрузка JSON - Чтение ~/files/terminals.json.
- Десериализация - JsonSerializer с case-insensitive (System.Text.Json).
- Очистка данных - Удаление существующих записей в БД.
- Импорт данных - Bulk insert новых терминалов.
- Индексы в БД.
- Логирование - Структурированные логи всех операций.
- Обработка ошибок.

#### Пример логов

```log
{Time} INFO: Загружено {Count} терминалов из JSON
{Time} INFO: Удалено {OldCount} старых записей
{Time} INFO: Сохранено {NewCount} новых терминалов
{Time} ERROR: Ошибка импорта: {Exception}
```

### Нефункциональные требования

Время импорта < 5 минут

## Критерии приемки

- BackgroundService запускается и работает стабильно
- Данные корректно импортируются из JSON в PostgreSQL
- Структурированные логи в консоли
- Время импорта < 5 мин
- Graceful shutdown при остановке
- Обработка ошибок без краха сервиса
- Таблица  offices  содержит актуальные данные
- Методы апи возвращают результат

## Примерный сценарий использования данных

- Поиск офисов по идентификатору города - возвращает список офисов\пвз\терминалов
- Поиск офисов по названию города - возвращает список офисов\пвз\терминалов

## *

- Можно ли использовать сторонние библиотеки?
Задачу можно выполнить не используя сторонних библиотек, следственно использовать сторонние библиотеки нежелательно

- Надо очищать всю таблицу перед тем как обновлять данные?
Вы можете проверять существующие записи и обновлять их, или можете удалить запись целиком, записав новую, или очистить таблицу целиком и записать данные по новой. На ваше усмотрение.

- Как вернуть результаты?

1. Опубликовав результат на github и предоставить ссылку на репозиторий
2. Запаковать проект в zip и отправить
3. Pull request в проект

- Могу я добавить docker\docker compose?

Да можете, но docker не является критерием приемки
