using Application.Mediatr.Commands.Products;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Products;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Product?>
{
    private readonly ApiDataContext _apiDataContext;

    public CreateProductHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var serializedData = ProtoBufSerializer.ClassToByteArray(request.Product);

        var document = new BsonDocument
        {
            { "_id", Guid.NewGuid().ToString() },
            { "protobufData", serializedData }
        };

        await _apiDataContext.ProductsDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        return request.Product;
    }
}