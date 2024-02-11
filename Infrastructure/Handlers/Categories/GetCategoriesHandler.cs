using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Entities.Category;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<Category>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetCategoriesHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<IEnumerable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories =
            await (await _apiDataContext.CategoriesDocuments.FindAsync(new BsonDocument(),
                cancellationToken: cancellationToken)).ToListAsync(cancellationToken);
        var deserializedCategories = categories
            .Select(document =>
            {
                var categoryId = document["_id"];
                var protobufData = document["protobufData"].AsByteArray;

                var category = ProtoBufSerializer.ByteArrayToClass<Category>(protobufData);
                category.CategoryId = categoryId.AsString;

                return category;
            })
            .AsEnumerable();

        return deserializedCategories;
    }
}