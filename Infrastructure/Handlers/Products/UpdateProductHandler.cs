using Application.Mediatr.Commands.Products;
using Application.Mediatr.Queries.Categories;
using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Products;

public sealed class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IMediator _mediator;

    public UpdateProductHandler(ApiDataContext apiDataContext, IMediator mediator)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
    }
    
    public async Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId), cancellationToken);

        if (product is null)
            return null;
        
        var category = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId), cancellationToken);

        if (category is null)
            return null;
        
        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.ProductId);

        Product updateProduct = new()
        {
            ProductId = request.ProductId,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(updateProduct);

        var update = Builders<BsonDocument>.Update.Set("protobufData", serializedData);

        var result = await _apiDataContext.ProductsDocuments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        
        if (result.ModifiedCount != 0)
            return updateProduct;

        return null;
    }
}