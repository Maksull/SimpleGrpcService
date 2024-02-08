using Calzolari.Grpc.AspNetCore.Validation;
using System.IO.Compression;
using System.Text;
using Application.Validators.Categories;
using FluentValidation;
using Grpc.Net.Compression;
using GrpcService.Compression;
using GrpcService.Endpoints;
using GrpcService.Interceptors.ExceptionInterceptor;
using GrpcService.Mapster;
using GrpcService.Validators.v2.Categories;
using Infrastructure.Behaviors;
using Infrastructure.Data;
using Infrastructure.Handlers.Products;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using v1 = GrpcService.Services.v1;
using v2 = GrpcService.Services.v2;
using Infrastructure.Services.Interfaces;
using Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.FromSeconds(5),
    };
});
builder.Services.AddAuthorization();

builder.Services.AddGrpc(opts =>
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

builder.Services.AddGrpcValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();

builder.Services.AddSingleton<ApiDataContext>(_ =>
{
    string mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB")!;
    string databaseName = builder.Configuration["MongoDB:DatabaseName"]!;

    return new ApiDataContext(mongoConnectionString, databaseName);
});

TypeAdapterConfig config = new();
config.Apply(new MapsterRegister());
builder.Services.AddSingleton(config);

builder.Services.AddSingleton<IMapper>(sp => new ServiceMapper(sp, config));

builder.Services.AddValidatorsFromAssembly(typeof(GetCategoryByIdQueryValidator).Assembly);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<GetProductsHandler>();
    cfg.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});


builder.Services.AddStackExchangeRedisCache(opts =>
{
    opts.Configuration = builder.Configuration.GetConnectionString("RedisCache");
});

builder.Services.AddSingleton<ICacheService, CacheService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapProtosEndpoints();

app.MapGrpcService<v1.GrpcAuthService>();
app.MapGrpcService<v1.GrpcProductService>();
app.MapGrpcService<v1.GrpcCategoryService>();

app.MapGrpcService<v2.GrpcAuthService>();
app.MapGrpcService<v2.GrpcProductService>();
app.MapGrpcService<v2.GrpcCategoryService>();

app.Run();