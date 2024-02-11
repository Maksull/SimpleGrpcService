using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Entities.Product;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Products;

public sealed class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly ApiDataContext _apiDataContext;

    public GetProductByIdHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.Id);

        var response = await _apiDataContext.ProductsDocuments.FindAsync(filter, cancellationToken: cancellationToken);

        var protobufProduct = await response.FirstOrDefaultAsync(cancellationToken);

        if (protobufProduct is null) return null;

        var product = ProtoBufSerializer.ByteArrayToClass<Product>(protobufProduct["protobufData"].AsByteArray);
        product.ProductId = protobufProduct["_id"].AsString;

        return product;
    }
}