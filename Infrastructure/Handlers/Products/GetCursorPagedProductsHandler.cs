using Application.Mediatr.Queries.Products;
using Application.Serialization;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infrastructure.Handlers.Products;

public sealed class GetCursorPagedProductsHandler : IRequestHandler<GetCursorPagedProductsQuery, CursorPagedResponse<Product>>
{
    private readonly ApiDataContext _apiDataContext;

    public GetCursorPagedProductsHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }

    public async Task<CursorPagedResponse<Product>> Handle(GetCursorPagedProductsQuery request, CancellationToken cancellationToken)
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

        productsQuery = productsQuery.Where(c => c["_id"] > request.Cursor).Take(request.PageSize);

        var products = await productsQuery.ToListAsync(cancellationToken);
        var deserializedProducts = products
            .Select(document =>
            {
                var protobufData = document["protobufData"].AsByteArray;

                var product = ProtoBufSerializer.ByteArrayToClass<Product>(protobufData);
                product.ProductId = document["_id"].AsString;

                return product;
            }).ToList();

        return new CursorPagedResponse<Product>(deserializedProducts,
            deserializedProducts.LastOrDefault()?.ProductId, request.PageSize);
    }
}