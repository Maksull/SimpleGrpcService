using MediatR;

namespace Application.Mediatr.Notifications.Categories;

public sealed record CategoryCreated() : INotification;
