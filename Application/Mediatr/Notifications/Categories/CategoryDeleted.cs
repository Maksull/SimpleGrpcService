using MediatR;

namespace Application.Mediatr.Notifications.Categories;

public sealed record CategoryDeleted(string Id) : INotification;
