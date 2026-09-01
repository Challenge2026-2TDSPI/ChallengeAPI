using System.Diagnostics.Metrics;

namespace ChallengeAPI.Telemetry;

public static class AppMetrics
{
    public static readonly Meter Meter = new("ChallengeAPI", "1.0.0");

    public static readonly Counter<long> RequestsCounter =
        Meter.CreateCounter<long>(
            "challengeapi.requests",
            description: "Quantidade de operações executadas pela API.");

    public static readonly Histogram<double> RequestDuration =
        Meter.CreateHistogram<double>(
            "challengeapi.request.duration",
            unit: "ms",
            description: "Duração das operações executadas pela API.");
}
