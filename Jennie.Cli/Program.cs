using Jennie.Errors;

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

foreach (var configLine in configLines)
{
    Console.WriteLine(configLine);
}

return 0;