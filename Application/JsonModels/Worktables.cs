using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class Worktables
{
    [JsonPropertyName("specialWorktable")]
    public SpecialWorktable? SpecialWorktable { get; set; }

    [JsonPropertyName("worktable")]
    public List<WorktableEntry>? Worktable { get; set; }
}