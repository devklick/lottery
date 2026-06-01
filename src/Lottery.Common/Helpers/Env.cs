namespace Lottery.Common.Helpers;

public static class Env
{
    public static string GetRequiredEnvVar(string name)
        => Environment.GetEnvironmentVariable(name)
        ?? throw new KeyNotFoundException($"No environment variable with name {name} could be found");
}