# ТЗ

Реализовать справочник терминалов.

## Задача

Создать фоновую службу (BackgroundService) для периодической загрузки справочника терминалов из JSON файла и сохранения данных в PostgreSQL

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
    public Address Address { get; set; }
    public string WorkTime { get; set; }
    public List<Phone> Phones { get; set; } = new List<Phone>();
    public Office() { }
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
