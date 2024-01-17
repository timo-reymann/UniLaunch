namespace UniLaunch.Core.Targets;

public enum TargetInvokeResultStatus
{
    Success,
    Failure,
    Warning,
}

public record Warning(string Key, string? Details = null);

public record Error(string Key, string? Details = null);

public record TargetInvokeResult(
    Target Target,
    TargetInvokeResultStatus Status,
    Warning[]? Warnings = null,
    Error[]? Errors = null
)
{
    public void Print()
    {
        Console.Write($"{Target.Name} => {Status}");
        if (Errors == null)
        {
            return;
        }

        Console.Write("( ");
        foreach (var resultError in Errors)
        {
            Console.Write(resultError.Details?.Trim() ?? "N/A");
        }

        Console.Write(" )");

        Console.WriteLine();
    }
}