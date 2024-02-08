using MediatR;

namespace Application.Mediatr.Notifications.Products;

public sealed record ProductCreated() : INotification;
