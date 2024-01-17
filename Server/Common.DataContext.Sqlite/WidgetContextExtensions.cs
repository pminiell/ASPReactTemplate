using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common.DataContext.Sqlite;

public static class WidgetContextExtensions
{
    public static IServiceCollection AddWidgetContext(this IServiceCollection services, string relativePath = "..")
    {
        string databasePath = Path.Combine(relativePath, "database.db");

        services.AddDbContext<WidgetContext>(options =>
        {
            options.UseSqlite($"Data Source={databasePath}");
        });
        return services;
    }
}