namespace Jennie.Errors;

public sealed record Error(int Code, string Description)
{
    public static Error InvalidCommand() => new(1, "No command given");
    public static Error InvalidCommand(string value) => new(2, $"Invalid command '{value}' given");
    public static Error ConfigFileNotFound() => new(3, "No jennie.config file was found in current directory");
    public static Error InvalidConfigLine(string configLine) => new(4, $"Invalid config line -> {configLine}");

    public override string ToString() => $"Error: {Description} ({Code})";
}