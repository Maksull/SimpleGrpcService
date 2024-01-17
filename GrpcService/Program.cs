using Application.Validators.Categories;
using FluentValidation;
using GrpcService.Mapster;
using GrpcService.Services;
using Infrastructure.Behaviors;
using Infrastructure.Data;
using Infrastructure.Handlers.Products;
using Mapster;
using MapsterMapper;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

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
});
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

var app = builder.Build();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<GrpcProductService>();
app.MapGrpcService<GrpcCategoryService>();

app.Run();