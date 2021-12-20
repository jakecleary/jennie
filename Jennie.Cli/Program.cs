using Jennie.Errors;
using MoreLinq;

if (!args.Any())
{
    var error = Error.InvalidCommand();
    Console.WriteLine(error);
    return error.Code;
}

var command = args[0];

if (command != "build")
{
    var error = Error.InvalidCommand(command);
    Console.WriteLine(error);
    return error.Code;
}

const string configFileName = "jennie.config";

if (!File.Exists(configFileName))
{
    var error = Error.ConfigFileNotFound();
    Console.WriteLine(error);
    return error.Code;
}

var configLines = await File.ReadAllLinesAsync(configFileName);

Console.WriteLine($"{configFileName} contains the following:");
Console.WriteLine();

var errors = new List<Error>();
var configValues = new Dictionary<string, string>();

foreach (var configLine in configLines)
{
    var parts = configLine.Split(": ");

    if (parts.Length != 2)
    {
        errors.Add(Error.InvalidConfigLine(configLine));
    }
    else
    {
        var key = parts[0].Trim();
        var value = parts[1].Trim(' ', '"');
        configValues.Add(key, value);
    }
}

if (errors.Any())
{
    errors.ForEach(Console.WriteLine);

    return errors.First().Code;
}

configValues.ForEach(kvp => Console.WriteLine($"{kvp.Key}: {kvp.Value}"));

return 0;