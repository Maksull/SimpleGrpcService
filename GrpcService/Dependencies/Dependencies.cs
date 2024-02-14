using System.IO.Compression;
using System.Text;
using Calzolari.Grpc.AspNetCore.Validation;
using FluentValidation;
using Grpc.Net.Compression;
using GrpcService.Compression;
using GrpcService.Interceptors.ExceptionInterceptor;
using GrpcService.Mapster;
using GrpcService.Validators.v2.Categories;
using Infrastructure.BackgroundJobs;
using Infrastructure.Behaviors;
using Infrastructure.Data;
using Infrastructure.Handlers.Products;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace GrpcService.Dependencies;

public static class Dependencies
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureDataContext(configuration);
        services.ConfigureGrpc();
        services.ConfigureAuthenticationAuthorization(configuration);
        services.ConfigureFluentValidation();
        services.ConfigureMediatR();
        services.ConfigureMapster();
        services.ConfigureQuartz();
        services.ConfigureCache(configuration);

        return services;
    }

    private static void ConfigureDataContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ApiDataContext>(_ =>
        {
            string mongoConnectionString = configuration.GetConnectionString("MongoDB")!;
            string databaseName = configuration["MongoDB:DatabaseName"]!;

            return new ApiDataContext(mongoConnectionString, databaseName);
        });
    }
    
    private static void ConfigureGrpc(this IServiceCollection services)
    {
        services.AddGrpc(opts =>
        {
            opts.MaxReceiveMessageSize = 6291456; // 6 MB
            opts.MaxSendMessageSize = 6291456; // 6 MB
            opts.CompressionProviders = new List<Grpc.Net.Compression.ICompressionProvider>
            {
                new GzipCompressionProvider(CompressionLevel.Fastest), // gzip
                new BrotliCompressionProvider(CompressionLevel.Fastest) // br
            };
            opts.ResponseCompressionAlgorithm = "gzip";
            opts.ResponseCompressionLevel = CompressionLevel.Fastest; // compression level used if not set on the provider

            opts.Interceptors.Add<ExceptionInterceptor>();
            opts.EnableMessageValidation();
        });
        
        services.AddGrpcValidation();
    }
    
    private static void ConfigureAuthenticationAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecurityKey"]!)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.FromSeconds(5),
            };
        });
        services.AddAuthorization();
    }
    
    private static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
    }
    
    private static void ConfigureMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<GetProductsHandler>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            cfg.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
        });
    }
    
    private static void ConfigureMapster(this IServiceCollection services)
    {
        TypeAdapterConfig config = new();
        config.Apply(new MapsterRegister());
        services.AddSingleton(config);

        services.AddSingleton<IMapper>(sp => new ServiceMapper(sp, config));
    }
    
    private static void ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(opts =>
        {
            var jobKeyCategories = JobKey.Create(nameof(DeleteCategoriesPermanentlyJob));
            var jobKeyProducts = JobKey.Create(nameof(DeleteProductsPermanentlyJob));

            opts.AddJob<DeleteCategoriesPermanentlyJob>(jobKeyCategories)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKeyCategories).WithSimpleSchedule(schedule => schedule.RepeatForever().WithIntervalInSeconds(4)));
            opts.AddJob<DeleteProductsPermanentlyJob>(jobKeyProducts)
                .AddTrigger(trigger =>
                    trigger.ForJob(jobKeyProducts).WithSimpleSchedule(schedule => schedule.RepeatForever().WithIntervalInSeconds(4)));
        });

        services.AddQuartzHostedService();
    }
    
    private static void ConfigureCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(opts =>
        {
            opts.Configuration = configuration.GetConnectionString("RedisCache");
        });

        services.AddSingleton<ICacheService, CacheService>();
    }
}