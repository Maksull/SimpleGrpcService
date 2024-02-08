﻿using Application.Mediatr.Generics;
using Domain.Entities;

namespace Application.Mediatr.Queries.Categories;

public sealed record GetCategoryByIdQuery(string Id) : ICachedQuery<Category?>
{
    public string Key => $"category-by-id-{Id}";

    public TimeSpan? Expiration => null;
}
