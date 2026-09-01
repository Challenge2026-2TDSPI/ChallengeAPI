using Xunit;

namespace ChallengeAPI.IntegrationTests;

[CollectionDefinition(Name)]
public class ApiCollection : ICollectionFixture<ApiFactory>
{
    public const string Name = "API Collection";
}
