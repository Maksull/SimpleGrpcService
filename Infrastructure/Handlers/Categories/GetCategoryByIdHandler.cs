using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly ApiDataContext _apiDataContext;

    public GetCategoryByIdHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.Id);

        var response = await _apiDataContext.CategoriesDocuments.FindAsync(filter, cancellationToken: cancellationToken);

        var protobufCategory = await response.FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (protobufCategory is null) return null;
        
        var category = ProtoBufSerializer.ByteArrayToClass<Category>(protobufCategory["protobufData"].AsByteArray);
        category.CategoryId = protobufCategory["_id"].AsString;

        return category;
    }
}