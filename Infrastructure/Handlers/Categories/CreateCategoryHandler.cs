using Application.Mediatr.Commands.Categories;
using Application.Mediatr.Notifications.Categories;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IPublisher _publisher;

    public CreateCategoryHandler(ApiDataContext apiDataContext, IPublisher publisher)
    {
        _apiDataContext = apiDataContext;
        _publisher = publisher;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category newCategory = new()
        {
            CategoryId = Ulid.NewUlid().ToString(),
            Name = request.CategoryName,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(newCategory);

        var document = new BsonDocument
        {
            { "_id", newCategory.CategoryId },
            { "protobufData", serializedData }
        };

        await _apiDataContext.CategoriesDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        await _publisher.Publish(new CategoryCreated(), cancellationToken);

        return newCategory;
    }
}