﻿using Domain.Entities;
using Domain.Entities.Category;
using MediatR;

namespace Application.Mediatr.Commands.Categories;

public sealed record UpdateCategoryCommand(string CategoryId, string CategoryName) : IRequest<Category?>;