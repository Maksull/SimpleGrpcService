using GrpcService.Mapster;
using GrpcService.Services;
using Infrastructure.Data;
using Infrastructure.Handlers.Products;
using Mapster;
using MapsterMapper;

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

builder.Services.AddSingleton<IMapper>(sp =>
{
    return new ServiceMapper(sp, config);
});

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetProductsHandler>());

var app = builder.Build();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.MapGrpcService<GrpcProductService>();
app.MapGrpcService<GrpcCategoryService>();

app.Run();
