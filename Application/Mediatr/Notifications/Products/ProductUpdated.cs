using MediatR;

namespace Application.Mediatr.Notifications.Products;

public sealed record ProductUpdated(string Id) : INotification;
