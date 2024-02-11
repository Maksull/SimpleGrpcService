using Application.Mediatr.Commands.Products;
using Application.Mediatr.Notifications.Products;
using Application.Serialization;
using Domain.Entities.Product;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Products;

public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, Product?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IPublisher _publisher;

    public CreateProductHandler(ApiDataContext apiDataContext, IPublisher publisher)
    {
        _apiDataContext = apiDataContext;
        _publisher = publisher;
    }

    public async Task<Product?> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        Product newProduct = new()
        {
            ProductId = Ulid.NewUlid().ToString(),
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(newProduct);

        var document = new BsonDocument
        {
            { "_id", newProduct.ProductId },
            { "protobufData", serializedData }
        };

        await _apiDataContext.ProductsDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        await _publisher.Publish(new ProductCreated(), cancellationToken);

        return newProduct;
    }
}