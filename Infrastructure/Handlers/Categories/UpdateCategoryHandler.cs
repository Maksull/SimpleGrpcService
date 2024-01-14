using Application.Mediatr.Commands.Categories;
using Application.Mediatr.Queries.Categories;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Handlers.Categories;

public sealed class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Category?>
{
    private readonly ApiDataContext _apiDataContext;
    private readonly IMediator _mediator;

    public UpdateCategoryHandler(ApiDataContext apiDataContext, IMediator mediator)
    {
        _apiDataContext = apiDataContext;
        _mediator = mediator;
    }

    public async Task<Category?> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _mediator.Send(new GetCategoryByIdQuery(request.Category.CategoryId), cancellationToken);

        if (category is null)
            return null;
        
        var filter = Builders<BsonDocument>.Filter.Eq("_id", request.Category.CategoryId);

        var serializedData = ProtoBufSerializer.ClassToByteArray(request.Category);

        var update = Builders<BsonDocument>.Update.Set("protobufData", serializedData);

        var result = await _apiDataContext.CategoriesDocuments.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

        if (result.ModifiedCount != 0)
            return request.Category;

        return null;
    }
}