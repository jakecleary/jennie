using Jennie.Core;
using Jennie.Errors;
using MoreLinq;

const string configFileName = "jennie.config";

if (!args.Any())
{
    var error = Error.InvalidCommand();
    Console.WriteLine(error);
    return error.Code;
}

return args[0] switch
{
    "new" => await ProcessNewProjectCommand(args[1..]),
    "build" => await ProcessBuildCommand(),
    _ => await ProcessInvalidCommand(args[0]),
};

static async Task<int> ProcessNewProjectCommand(string[] args)
{
    if (!args.Any())
    {
        var error = Error.InvalidCommandArguments("new", string.Join(' ', args));
        Console.WriteLine(error);
        return error.Code;
    }

    var projectName = args.First().Trim();

    if (string.IsNullOrWhiteSpace(projectName))
    {
        var error = Error.InvalidConfigValueSupplied("name");
        Console.WriteLine(error);
        return error.Code;
    }

    var project = new ProjectConfig(projectName);

    if (File.Exists(configFileName))
    {
        var error = Error.ProjectAlreadyInitialized();
        Console.WriteLine(error);
        return error.Code;
    }

    await using StreamWriter sw = File.CreateText(configFileName);

    sw.WriteLine("name: {0}", project.ProjectName);

    Console.WriteLine($"New project '{project.ProjectName}' initialized!");

    return 0;
}

static async Task<int> ProcessBuildCommand()
{
    if (!File.Exists(configFileName))
    {
        var error = Error.ConfigFileNotFound();
        Console.WriteLine(error);
        return error.Code;
    }

    var configLines = await File.ReadAllLinesAsync(configFileName);

    var errors = new List<Error>();
    var configValues = new Dictionary<string, string>();

    for (var i = 0; i < configLines.Length; i++)
    {
        var lineNumber = i + 1;
        var lineContents = configLines[i];
        var partsParts = lineContents.Split(": ");

        if (partsParts.Length != 2)
        {
            errors.Add(Error.InvalidConfigLine(lineNumber, lineContents));
            continue;
        }

        var key = partsParts[0].Trim();
        var value = partsParts[1].Trim(' ', '"');
        configValues.Add(key, value);
    }

    if (errors.Any())
    {
        Console.WriteLine($"{configFileName} is invalid!");
        Console.WriteLine();
        errors.ForEach(Console.WriteLine);

        return errors.First().Code;
    }

    Console.WriteLine($"{configFileName} contains the following:");
    Console.WriteLine();
    configValues.ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));

    return 0;
}

static Task<int> ProcessInvalidCommand(string command)
{
    var error = Error.InvalidCommand(command);
    Console.WriteLine(error);
    return Task.FromResult(error.Code);
}