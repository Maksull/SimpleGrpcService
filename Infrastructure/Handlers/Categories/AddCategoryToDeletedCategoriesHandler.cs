using Application.Mediatr.Commands.Categories;
using Application.Serialization;
using Domain.Entities.Category;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Categories;

public sealed class AddCategoryToDeletedCategoriesHandler : IRequestHandler<AddCategoryToDeletedCategoriesCommand, DeletedCategory?>
{
    private readonly ApiDataContext _apiDataContext;

    public AddCategoryToDeletedCategoriesHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<DeletedCategory?> Handle(AddCategoryToDeletedCategoriesCommand request, CancellationToken cancellationToken)
    {
        DeletedCategory deletedCategory = new()
        {
            DeletedCategoryId = Ulid.NewUlid().ToString(),
            CategoryId = request.Category.CategoryId,
            Name = request.Category.Name,
            DeletedAt = DateTime.UtcNow,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(deletedCategory);

        var document = new BsonDocument
        {
            { "_id", deletedCategory.DeletedCategoryId },
            { "protobufData", serializedData },
            { "DeletedAt", deletedCategory.DeletedAt}
        };

        await _apiDataContext.DeletedCategoriesDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        return deletedCategory;
    }
}