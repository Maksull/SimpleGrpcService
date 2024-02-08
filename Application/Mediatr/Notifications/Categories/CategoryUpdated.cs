﻿using MediatR;

namespace Application.Mediatr.Notifications.Categories;

public sealed record CategoryUpdated(string Id) : INotification;
