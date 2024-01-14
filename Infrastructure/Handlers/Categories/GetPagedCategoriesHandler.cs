using System.Linq.Expressions;
using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Handlers.Categories;

public sealed class GetPagedCategoriesHandler : IRequestHandler<GetPagedCategoriesQuery, PagedResponse<Category>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetPagedCategoriesHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<PagedResponse<Category>> Handle(GetPagedCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categoriesQuery = _apiDataContext.CategoriesDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermFilter =
                Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(request.SearchTerm, "i"));
            categoriesQuery = categoriesQuery.Where(c => searchTermFilter.Inject());
        }

        Expression<Func<BsonDocument, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "name" => category => category["name"],
            _ => category => category["_id"],
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(keySelector);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(keySelector);
        }

        int totalCount = await categoriesQuery.CountAsync(cancellationToken);
        categoriesQuery = categoriesQuery.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

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

        return new PagedResponse<Category>(deserializedCategories,
            request.Page, request.PageSize, totalCount);
    }
}