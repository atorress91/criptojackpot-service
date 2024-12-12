using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Asp.Versioning;
using CryptoJackpotService.Core.Mapper;
using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Models.Configuration;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Npgsql;

namespace CryptoJackpotService.Ioc;

public static class IocExtensionApp
{
    public static void IocAppInjectDependencies(this IServiceCollection services, string[]? args = null)
    {
        InjectAuthentication(services);
        InjectConfiguration(services);
        InjectSwagger(services);
        InjectDatabases(services);
        InjectLogging(services);
        InjectControllersAndDocumentation(services);
        InjectRepositories(services);
        InjectServices(services);
        InjectPackages(services);
        InjectRegisterServiceProvider(services);
        InjectSingletonAndFactories(services);
    }

    private static void InjectAuthentication(IServiceCollection services)
        => services.AddAuthentication().AddJwtBearer();

    private static void InjectConfiguration(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var env = serviceProvider.GetRequiredService<IHostEnvironment>();
        var lowerCaseEnvironment = env.EnvironmentName.ToLower();
        var executableLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (executableLocation == null) return;
        var builder = new ConfigurationBuilder()
            .SetBasePath(executableLocation)
            .AddJsonFile($"appsettings.{lowerCaseEnvironment}.json", false, true)
            .AddEnvironmentVariables();

        var configuration = builder.Build();
        var appSettingsSection = configuration.GetSection("AppSettings");

        services.Configure<ApplicationConfiguration>(appSettingsSection);
        services.AddSingleton(configuration);
    }

    private static void InjectSwagger(IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoJackpotService", Version = "v1" });
        });

    private static void InjectDatabases(IServiceCollection services)
    {
        var appConfig = services.BuildServiceProvider().GetRequiredService<IOptions<ApplicationConfiguration>>().Value;

        var connectionString = appConfig.ConnectionStrings?.PostgreSqlConnection;

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CryptoJackpotDbContext>(options =>
        {
            options.UseNpgsql(dataSource)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
    }

    private static void InjectLogging(IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var env = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        var lowerCaseEnvironment = env.EnvironmentName.ToLower();

        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddNLog($"$nlog.{lowerCaseEnvironment}.config");
        });
    }

    private static void InjectControllersAndDocumentation(IServiceCollection services, int majorVersion = 1,
        int minorVersion = 0)
    {
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(["text/plain"]);
        });

        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver();
        });

        services.AddFluentValidationAutoValidation();
        services.AddFluentValidationClientsideAdapters();
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
            config.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    private static void InjectRepositories(IServiceCollection services)
    {
    }

    private static void InjectServices(IServiceCollection services)
    {
    }

    private static void InjectPackages(IServiceCollection services)
        => services.AddAutoMapper(x => { x.AddProfile(new MapperProfile()); });

    private static void InjectRegisterServiceProvider(IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton(services.BuildServiceProvider());
    }

    private static void InjectSingletonAndFactories(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }
}