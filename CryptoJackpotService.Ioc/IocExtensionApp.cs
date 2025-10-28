using System.Globalization;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Asp.Versioning;
using CryptoJackpotService.Core.Filters;
using CryptoJackpotService.Core.Mapper;
using CryptoJackpotService.Core.Providers;
using CryptoJackpotService.Core.Providers.IProviders;
using CryptoJackpotService.Core.Services;
using CryptoJackpotService.Core.Services.IServices;
using CryptoJackpotService.Core.Validators;
using CryptoJackpotService.Data.Database;
using CryptoJackpotService.Data.Repositories;
using CryptoJackpotService.Data.Repositories.IRepositories;
using CryptoJackpotService.Models.Configuration;
using CryptoJackpotService.Models.Request.Auth;
using CryptoJackpotService.Models.Request.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
using Npgsql;

namespace CryptoJackpotService.Ioc;

public static class IocExtensionApp
{
    public static void IocAppInjectDependencies(this IServiceCollection services, IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        InjectConfiguration(services, configuration);
        InjectLocalization(services);
        InjectAuthentication(services, configuration);
        InjectDatabases(services, configuration);
        InjectLogging(services, environment);
        InjectSwagger(services);
        InjectControllersAndDocumentation(services);
        InjectServices(services);
        InjectValidators(services);
        InjectRepositories(services);
        InjectPackages(services);
        InjectSingletonAndFactories(services);
    }
    
    private static void InjectLocalization(IServiceCollection services)
    {
        services.AddLocalization();

        var supportedCultures = new[] { "en", "es" };
        var localizationOptions = new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("en"),
            SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
            SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList()
        };

        // Configurar para usar header Accept-Language
        localizationOptions.RequestCultureProviders.Clear();
        localizationOptions.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());

        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.DefaultRequestCulture = localizationOptions.DefaultRequestCulture;
            options.SupportedCultures = localizationOptions.SupportedCultures;
            options.SupportedUICultures = localizationOptions.SupportedUICultures;
            options.RequestCultureProviders = localizationOptions.RequestCultureProviders;
        });
    }

    private static void InjectAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var appConfig = configuration.GetSection("AppSettings").Get<ApplicationConfiguration>();

        if (appConfig?.JwtSettings?.SecretKey == null)
            throw new InvalidOperationException(
                "The JWT secret key cannot be null. Check your configuration.");

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                if (appConfig.JwtSettings != null)
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = appConfig.JwtSettings.Issuer,
                        ValidAudience = appConfig.JwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(appConfig.JwtSettings.SecretKey))
                    };
            });
    }

    private static void InjectConfiguration(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationConfiguration>(configuration.GetSection("AppSettings"));
    }

    private static void InjectSwagger(IServiceCollection services)
        => services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoJackpotService", Version = "v1" });
        });

    private static void InjectDatabases(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("AppSettings:ConnectionStrings:PostgreSqlConnection");
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                "The connection string 'PostgreSqlConnection' was not found or is empty. " +
                "Make sure it is defined in your .env or appsettings.json file with the correct path (AppSettings__ConnectionStrings__PostgreSqlConnection).");
        }

        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<CryptoJackpotDbContext>(options =>
        {
            options.UseNpgsql(dataSource)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
    }

    private static void InjectLogging(IServiceCollection services, IWebHostEnvironment environment)
    {
        var lowerCaseEnvironment = environment.EnvironmentName.ToLower();

        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddConsole();
            config.AddDebug();
            config.SetMinimumLevel(LogLevel.Debug);

            config.AddNLog($"nlog.{lowerCaseEnvironment}.config");
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

        services.AddControllers(options => { options.Filters.Add<LocalizedValidationFilter>(); })
            .AddJsonOptions(options =>
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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IUserReferralRepository, UserReferralRepository>();
    }

    private static void InjectServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IBrevoService, BrevoService>();
        services.AddScoped<IDigitalOceanStorageService, DigitalOceanStorageService>();
        services.AddScoped<IUserReferralService, UserReferralService>();

        services.AddScoped<IEmailTemplateProvider, EmailTemplateProvider>();
        services.AddScoped(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));
    }

    private static void InjectValidators(IServiceCollection services)
    {
        services.AddTransient<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddTransient<IValidator<AuthenticateRequest>, AuthenticatedRequestValidator>();
        services.AddTransient<IValidator<UpdateImageProfileRequest>, UpdateImageProfileRequestValidator>();
        services.AddTransient<IValidator<GenerateSecurityCodeRequest>, GenerateSecurityCodeRequestValidator>();
        services.AddTransient<IValidator<UpdatePasswordRequest>, UpdatePasswordRequestValidator>();
        services.AddTransient<IValidator<RequestPasswordResetRequest>, RequestPasswordResetRequestValidator>();
        services.AddTransient<IValidator<ResetPasswordWithCodeRequest>, ResetPasswordWithCodeRequestValidator>();
    }

    private static void InjectPackages(IServiceCollection services)
        => services.AddAutoMapper(x => { x.AddProfile(new MapperProfile()); });

    private static void InjectSingletonAndFactories(IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
    }
}