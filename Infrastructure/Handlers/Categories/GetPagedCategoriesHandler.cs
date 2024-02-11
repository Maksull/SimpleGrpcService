using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities.Category;
using Infrastructure.Data;
using MediatR;
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
        
        if (request.SortOrder?.ToLower() == "desc")
        {
            categoriesQuery = categoriesQuery.OrderByDescending(c => c["_id"]);
        }
        else
        {
            categoriesQuery = categoriesQuery.OrderBy(c => c["_id"]);
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