using Application.Mediatr.Commands.Categories;
using Application.Mediatr.Notifications.Categories;
using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Entities.Category;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IMediator _mediator;
    private readonly IPublisher _publisher;

    public UpdateCategoryHandler(ApiDataContext apiDataContext, IMediator mediator, IPublisher publisher)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
        _publisher = publisher;
    }

    public async Task<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(request.CategoryId), cancellationToken);

        if (category is null)
            return null;

        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.CategoryId);

        Category updateCategory = new()
        {
            CategoryId = request.CategoryId,
            Name = request.CategoryName,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(updateCategory);

        var update = Builders<BsonDocument>.Update.Set("protobufData", serializedData);

        var result = await _apiDataContext.CategoriesDocuments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount == 0)
            return null;

        await _publisher.Publish(new CategoryUpdated(request.CategoryId), cancellationToken);

        return updateCategory;
    }
}