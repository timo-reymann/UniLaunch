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
);