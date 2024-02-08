using Application.Mediatr.Commands.Categories;
using Application.Mediatr.Notifications.Categories;
using Application.Mediatr.Queries.Categories;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Category?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;

    public DeleteCategoryHandler(ApiDataContext apiDataContext, IMediator mediator, IPublisher publisher)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
        _publisher = publisher;
    }

    public async Task<Category?> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(request.Id), cancellationToken);

        if (category is null)
            return null;

        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.Id);

        var result = await _apiDataContext.CategoriesDocuments.DeleteOneAsync(filter, cancellationToken);

        if (result.DeletedCount == 0)
        {
            return null;
        }

        await _publisher.Publish(new CategoryDeleted(request.Id), cancellationToken);

        return category;
    }
}