using Common.EntityModels.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Common.DataContext.Sqlite;

public class WidgetContext : DbContext
{
    public WidgetContext() { }

    public WidgetContext(DbContextOptions<WidgetContext> options)
        : base(options) { }

    public DbSet<Widget> Widgets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder options) =>
        options.UseSqlite("Filename=../database.db");
}
