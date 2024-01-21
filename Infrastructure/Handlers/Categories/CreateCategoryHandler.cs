﻿using Application.Mediatr.Commands.Categories;
using Application.Serialization;
using Domain.Entities;
using Infrastructure.Data;
using MediatR;
using MongoDB.Bson;

namespace Infrastructure.Handlers.Categories;

public sealed class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ApiDataContext _apiDataContext;

    public CreateCategoryHandler(ApiDataContext apiDataContext)
    {
        _apiDataContext = apiDataContext;
    }
    
    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category newCategory = new()
        {
            CategoryId = Ulid.Empty.ToString(),
            Name = request.CategoryName,
        };

        var serializedData = ProtoBufSerializer.ClassToByteArray(newCategory);

        var document = new BsonDocument
        {
            { "_id", Ulid.NewUlid().ToString() },
            { "protobufData", serializedData }
        };

        await _apiDataContext.CategoriesDocuments.InsertOneAsync(document, cancellationToken: cancellationToken);

        return newCategory;
    }
}