using Application.Mediatr.Commands.Products;
using Application.Serialization;
using Domain.Entities.Product;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Products;

public sealed class
    AddProductToDeletedProductsHandler : IRequestHandler<AddProductToDeletedProductsCommand, DeletedProduct?>
{
    private readonly ApiDataContext _apiDataContext;

    public AddProductToDeletedProductsHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<DeletedProduct?> Handle(AddProductToDeletedProductsCommand request,
        CancellationToken cancellationToken)
    {
        DeletedProduct deletedProduct = new()
        {
            DeletedProductId = Ulid.NewUlid().ToString(),
            ProductId = request.Product.ProductId,
            Name = request.Product.Name,
            Description = request.Product.Description,
            CategoryId = request.Product.CategoryId,
            DeletedAt = DateTime.UtcNow,
            DeleteAt = DateTime.UtcNow.AddMinutes(5),
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(deletedProduct);

        var document = new BsonDocument
        {
            { "_id", deletedProduct.DeletedProductId },
            { "protobufData", serializedData },
            { "deletedAt", deletedProduct.DeletedAt },
            { "deleteAt", deletedProduct.DeleteAt },
        };

        await _apiDataContext.DeletedProductsDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        return deletedProduct;
    }
}