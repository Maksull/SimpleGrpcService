using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Entities.Product;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Products;

public sealed class GetProductsHandler : IRequestHandler<GetProductsQuery, IEnumerable<Product>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetProductsHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<IEnumerable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products =
            await (await _apiDataContext.ProductsDocuments.FindAsync(new BsonDocument(),
                cancellationToken: cancellationToken)).ToListAsync(cancellationToken);

        var deserializedProducts = products
            .Select(document =>
            {
                var protobufData = document["protobufData"].AsByteArray;

                var product = ProtoBufSerializer.ByteArrayToClass<Product>(protobufData);
                product.ProductId = document["_id"].AsString;

                return product;
            })
            .AsEnumerable();

        return deserializedProducts;
    }
}