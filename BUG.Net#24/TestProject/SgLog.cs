namespace TestProject;

public partial class SgLog
{
    [LoggerMessage(0, LogLevel.Information, "Job has {Status} processing")]
    public static partial void LogJobStatus(ILogger<SgLog> logger, string status);
}