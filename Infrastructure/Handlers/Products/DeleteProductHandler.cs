using Application.Mediatr.Commands.Products;
using Application.Mediatr.Notifications.Products;
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
    private readonly IPublisher _publisher;

    public DeleteProductHandler(ApiDataContext apiDataContext, IMediator mediator, IPublisher publisher)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
        _publisher = publisher;
    }

    public async Task<Product?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(request.Id), cancellationToken);

        if (product is null)
            return null;

        var filter = Builders<BsonDocument>.Filter.Eq("_id", product.ProductId);

        var result = await _apiDataContext.ProductsDocuments.DeleteOneAsync(filter, cancellationToken);

        if (result.DeletedCount == 0)
            return null;

        await _publisher.Publish(new ProductDeleted(request.Id), cancellationToken);

        return product;
    }
}