using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Eterna.Infrastructure.Persistence;

public sealed class EternaDbContextFactory : IDesignTimeDbContextFactory<EternaDbContext>
{
    public EternaDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
        var webPath = ResolveWebPath();

        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(webPath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true);

        AddWebUserSecrets(configurationBuilder, webPath);

        var configuration = configurationBuilder
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        var optionsBuilder = new DbContextOptionsBuilder<EternaDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new EternaDbContext(optionsBuilder.Options);
    }

    private static string ResolveWebPath()
    {
        var current = Directory.GetCurrentDirectory();
        var webPath = Path.GetFullPath(Path.Combine(current, "..", "Eterna.Web"));

        if (Directory.Exists(webPath) && File.Exists(Path.Combine(webPath, "appsettings.json")))
        {
            return webPath;
        }

        var fromRoot = Path.GetFullPath(Path.Combine(current, "src", "Eterna.Web"));
        if (Directory.Exists(fromRoot) && File.Exists(Path.Combine(fromRoot, "appsettings.json")))
        {
            return fromRoot;
        }

        return current;
    }

    private static void AddWebUserSecrets(IConfigurationBuilder builder, string webPath)
    {
        var projectFile = Path.Combine(webPath, "Eterna.Web.csproj");
        if (!File.Exists(projectFile))
        {
            return;
        }

        var match = Regex.Match(
            File.ReadAllText(projectFile),
            @"<UserSecretsId>([^<]+)</UserSecretsId>");

        if (!match.Success)
        {
            return;
        }

        var secretsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Microsoft",
            "UserSecrets",
            match.Groups[1].Value,
            "secrets.json");

        if (File.Exists(secretsPath))
        {
            builder.AddJsonFile(secretsPath, optional: true);
        }
    }
}
