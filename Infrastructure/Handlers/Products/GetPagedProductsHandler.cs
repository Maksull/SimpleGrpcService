using System.Linq.Expressions;
using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Handlers.Products;

public sealed class GetPagedProductsHandler : IRequestHandler<GetPagedProductsQuery, PagedResponse<Product>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetPagedProductsHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<PagedResponse<Product>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
    {
        var productsQuery = _apiDataContext.ProductsDocuments.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTermFilter =
                Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(request.SearchTerm, "i"));
            productsQuery = productsQuery.Where(c => searchTermFilter.Inject());
        }

        Expression<Func<BsonDocument, object>> keySelector = request.SortColumn?.ToLower() switch
        {
            "name" => category => category["name"],
            "description" => category => category["description"],
            "categoryId" => category => category["categoryId"],
            _ => category => category["_id"],
        };

        if (request.SortOrder?.ToLower() == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(keySelector);
        }
        else
        {
            productsQuery = productsQuery.OrderBy(keySelector);
        }

        int totalCount = await productsQuery.CountAsync(cancellationToken);
        productsQuery = productsQuery.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

        var products = await productsQuery.ToListAsync(cancellationToken);
        var deserializedProducts = products
            .Select(document =>
            {
                var protobufData = document["protobufData"].AsByteArray;

                var product = ProtoBufSerializer.ByteArrayToClass<Product>(protobufData);
                product.ProductId = document["_id"].AsString;

                return product;
            }).ToList();

        return new PagedResponse<Product>(deserializedProducts,
            request.Page, request.PageSize, totalCount);
    }
}