using Microsoft.Extensions.Options;

namespace StranaKart.Assignment.Infrastructure.Scheduling.Options;

internal sealed class SimpleScheduleOptionsValidator : IValidateOptions<SimpleScheduleOptions>
{
    public ValidateOptionsResult Validate(string? name, SimpleScheduleOptions? options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("SimpleScheduleOptions не найдены.");

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.TimeZone) ||
            !TimeZoneInfo.TryFindSystemTimeZoneById(options.TimeZone, out _))
            errors.Add($"{SimpleScheduleOptions.SectionName}:TimeZone — неизвестная или пустая таймзона '{options.TimeZone}'.");

        if (options.Hour is < 0 or > 23)
            errors.Add($"{SimpleScheduleOptions.SectionName}:Hour — значение {options.Hour} вне диапазона 0–23.");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
