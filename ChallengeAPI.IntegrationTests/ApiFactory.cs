
using ChallengeAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChallengeAPI.IntegrationTests;

public class ApiFactory : WebApplicationFactory<Program>
{
    static ApiFactory()
    {
        Environment.SetEnvironmentVariable(
            "Jwt__SecretKey",
            "TestSecretKeyThatIsAtLeast32CharactersLong!");

        Environment.SetEnvironmentVariable("Jwt__Issuer", "ChallengeAPI");
        Environment.SetEnvironmentVariable("Jwt__Audience", "ChallengeAPI");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<AppDbContext>>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("ChallengeAPIIntegrationTests"));
        });
    }
}
