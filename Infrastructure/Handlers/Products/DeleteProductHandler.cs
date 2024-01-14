using Application.Mediatr.Commands.Products;
using Application.Mediatr.Queries.Products;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Products;

public sealed class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Product?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IMediator _mediator;

    public DeleteProductHandler(ApiDataContext apiDataContext, IMediator mediator)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
    }

    public async Task<Product?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(request.Id), cancellationToken);

        if (product is null)
            return null;
        
        var filter = Builders<BsonDocument>.Filter.Eq("_id", product.ProductId);

        var result = await _apiDataContext.ProductsDocuments.DeleteOneAsync(filter, cancellationToken);
        
        if (result.DeletedCount != 0)
            return product;

        return null;
    }
}