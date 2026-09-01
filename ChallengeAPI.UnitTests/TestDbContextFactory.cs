using ChallengeAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ChallengeAPI.UnitTests;

public static class TestDbContextFactory
{
    public static AppDbContext Create(string databaseName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new AppDbContext(options);
    }
}
