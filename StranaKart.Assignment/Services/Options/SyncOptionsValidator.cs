using Microsoft.Extensions.Options;

namespace StranaKart.Assignment.Services.Options;

internal sealed class SyncOptionsValidator : IValidateOptions<SyncOptions>
{
    public ValidateOptionsResult Validate(string? name, SyncOptions? options)
    {
        if (options is null)
            return ValidateOptionsResult.Fail("SyncOptions не найдены.");

        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.FilePath))
            errors.Add($"{SyncOptions.SectionName}:FilePath — не может быть пустым.");

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
