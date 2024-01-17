using Common.DataContext.Sqlite;
using Common.WebApi.Repositories.WidgetRepository;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc.Formatters;

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddCors(options =>
    {
        options.AddPolicy(
            name: myAllowSpecificOrigins,
            policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }
        );
    });

builder.Services.AddWidgetContext();
builder
    .Services
    .AddControllers(options =>
    {
        Console.WriteLine("Default output formatters:");
        foreach (IOutputFormatter formatter in options.OutputFormatters)
        {
            OutputFormatter? mediaFormatter = formatter as OutputFormatter;
            if (mediaFormatter == null)
            {
                Console.WriteLine($" {formatter.GetType().Name}");
            }
            else // OutputFormatter class has SupportedMediaTypes
            {
                Console.WriteLine(
                    " {0}, Media types: {1}",
                    arg0: mediaFormatter.GetType().Name,
                    arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes)
                );
            }
        }
    });

builder
    .Services
    .AddHttpLogging(options =>
    {
        options.LoggingFields = HttpLoggingFields.All;
        options.RequestBodyLogLimit = 4096;
        options.ResponseBodyLogLimit = 4096;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWidgetRepository, WidgetRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);
app.MapControllers();

app.Run();
