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
        Product newProduct = new()
        {
            ProductId = Ulid.Empty.ToString(),
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(newProduct);

        var document = new BsonDocument
        {
            { "_id", Ulid.NewUlid().ToString() },
            { "protobufData", serializedData }
        };

        await _apiDataContext.ProductsDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        return newProduct;
    }
}