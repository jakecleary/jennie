namespace Jennie.Errors;

public sealed record Error(int Code, string Description)
{
    public static Error InvalidCommand() => new(1, "No command given");
    public static Error InvalidCommand(string value) => new(2, $"Invalid command '{value}' given");
    public static Error ConfigFileNotFound() => new(3, "No jennie.config file was found in current directory");
    public static Error InvalidConfigLine(int lineNumber, string lineContents) => new(4, $"Config line {lineNumber} is invalid with value '{lineContents}'");
    public static Error ProjectAlreadyInitialized() => new(5, "jennie project already initialized in the current directory");
    public static Error InvalidConfigValueSupplied(string configKey) => new(6, $"Config for '{configKey}' must be supplied");

    public static Error InvalidCommandArguments(string command, string arguments)
    {
        return string.IsNullOrWhiteSpace(arguments)
            ? new(7, $"Command '{command}' supplied with no arguments")
            : new(7, $"Command '{command}' supplied with invalid arguments '{arguments}'");
    }

    public override string ToString() => $"Error ({Code}): {Description}";
}