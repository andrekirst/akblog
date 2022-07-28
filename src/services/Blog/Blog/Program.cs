using Blog.Database;
using Blog.Database.Repositories;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Infrastructure.Database;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Blog;

public class Program
{
    private static IWebHostEnvironment? _environment;
    private static ConfigurationManager? _configuration;

    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _environment = builder.Environment;
        _configuration = builder.Configuration;

        builder.Services.AddProblemDetails(ConfigureProblemDetails);
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
        });
        builder.Services.AddProblemDetailsConventions();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default"));
        });
        var domainAssembly = typeof(Program).Assembly;
        builder.Services.AddMediatR(domainAssembly);
        builder.Services.AddValidatorsFromAssembly(domainAssembly);
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.Configure<MassTransitRabbitMqOptions>(options => _configuration.GetSection(nameof(MassTransitRabbitMqOptions)).Bind(options));

        builder.Services.AddMassTransit(config =>
        {
            config.AddBus(context =>
            {
                var options = context.GetRequiredService<IOptions<MassTransitRabbitMqOptions>>().Value;
                return Bus.Factory.CreateUsingRabbitMq(rabbitMq =>
                {
                    rabbitMq.Host(options.Host, options.VirtualHost, h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });

                    var serviceInstanceOptions = new ServiceInstanceOptions()
                        .SetEndpointNameFormatter(KebabCaseEndpointNameFormatter.Instance);

                    rabbitMq.ConfigureServiceEndpoints(context);
                });
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseProblemDetails();
        app.MapControllers();

        await MigrateDatabase(app);

        await app.RunAsync();
    }

    private static async Task MigrateDatabase(IHost app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await dbContext.Database.MigrateAsync();
        }
    }

    private static void ConfigureProblemDetails(ProblemDetailsOptions options)
    {
        options.IncludeExceptionDetails = (_, _) => _environment?.IsDevelopment() ?? false;
        options.MapFluentValidationException();
        options.Rethrow<NotSupportedException>();
        options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
        options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);
        options.MapToStatusCode<ValidationException>(StatusCodes.Status400BadRequest);
        options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    }
}

public class MassTransitRabbitMqOptions
{
    public string? Host { get; set; }
    public string VirtualHost { get; set; } = "/";
    public string? Username { get; set; }
    public string? Password { get; set; }
}