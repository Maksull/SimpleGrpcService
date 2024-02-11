using Domain.Entities.Category;
using Mapster;
using v1 = SimpleGrpcProject.v1;
using v2 = SimpleGrpcProject.v2;

namespace GrpcService.Mapster;

public sealed class MapsterRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        #region v1

        config.ForType<Domain.Entities.Product.Product, v1.Product>();
        config.ForType<v1.Product, Domain.Entities.Product.Product>();

        config.ForType<Category, v1.Category>();
        config.ForType<v1.Category, Category>();
        
        #endregion
        
        #region v2

        config.ForType<Domain.Entities.Product.Product, v2.Product>();
        config.ForType<v2.Product, Domain.Entities.Product.Product>();

        config.ForType<Category, v2.Category>();
        config.ForType<v2.Category, Category>();
        
        #endregion
    }
}