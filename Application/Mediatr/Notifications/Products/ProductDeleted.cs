using MediatR;

namespace Application.Mediatr.Notifications.Products;

public sealed record ProductDeleted(string Id) : INotification;
