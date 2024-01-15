using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Handlers.Categories;

public sealed class GetCursorPagedCategoriesHandler : IRequestHandler<GetCursorPagedCategoriesQuery, CursorPagedResponse<Category>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetCursorPagedCategoriesHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<CursorPagedResponse<Category>> Handle(GetCursorPagedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoriesQuery = _apiDataContext.CategoriesDocuments.AsQueryable();

        if (request.SortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(c => c["_id"]);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(c => c["_id"]);
        }

        categoriesQuery = categoriesQuery.Where(c => c["_id"] > request.Cursor).Take(request.PageSize);

        var categories = await categoriesQuery.ToListAsync(cancellationToken);
        var deserializedCategories = categories
            .Select(document =>
            {
                var categoryId = document["_id"];
                var protobufData = document["protobufData"].AsByteArray;

                var category = ProtoBufSerializer.ByteArrayToClass<Category>(protobufData);
                category.CategoryId = categoryId.AsString;

                return category;
            }).ToList();

        return new CursorPagedResponse<Category>(deserializedCategories,
            deserializedCategories.LastOrDefault()?.CategoryId, request.PageSize);
    }
}