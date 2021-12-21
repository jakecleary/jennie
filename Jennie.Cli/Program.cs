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