using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities.Product;
using Infrastructure.Data;
using MediatR;
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

        if (request.SortOrder?.ToLower() == "desc")
        {
            productsQuery = productsQuery.OrderByDescending(p => p["_id"]);
        }
        else
        {
            productsQuery = productsQuery.OrderBy(p => p["_id"]);
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