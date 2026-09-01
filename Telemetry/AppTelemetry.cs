using System.Diagnostics;

namespace ChallengeAPI.Telemetry;

public static class AppTelemetry
{
    public static readonly ActivitySource ActivitySource = new("ChallengeAPI");
}
