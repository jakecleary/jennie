namespace Jennie.Core;

public record ProjectConfig(string ProjectName)
{
    public override string ToString()
    {
        return $"name: {ProjectName}";
    }
}