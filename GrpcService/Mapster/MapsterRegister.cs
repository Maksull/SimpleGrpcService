using Mapster;
using SimpleGrpcProject;

namespace GrpcService.Mapster;

public sealed class MapsterRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Domain.Entities.Product, Product>();
        config.ForType<Product, Domain.Entities.Product>();
        
        config.ForType<Domain.Entities.Category, Category>();
        config.ForType<Category, Domain.Entities.Category>();
    }
}