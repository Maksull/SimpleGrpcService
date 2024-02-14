using GrpcService.Endpoints;
using GrpcService.Dependencies;
using v1 = GrpcService.Services.v1;
using v2 = GrpcService.Services.v2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

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