using Application.Mediatr.Notifications.Categories;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.NotificationHandlers.CacheInvalidation;

public sealed class CategoriesCacheInvalidationHandler : 
    INotificationHandler<CategoryDeleted>, 
    INotificationHandler<CategoryUpdated>, 
    INotificationHandler<CategoryCreated>
{
    private readonly ICacheService _cacheService;

    public CategoriesCacheInvalidationHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(CategoryDeleted notification, CancellationToken cancellationToken)
    {
        await InvalidateCategoriesCacheEntries(notification.Id);
    }

    public async Task Handle(CategoryUpdated notification, CancellationToken cancellationToken)
    {
        await InvalidateCategoriesCacheEntries(notification.Id);
    }

    public async Task Handle(CategoryCreated notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync("categories");
    }

    private async Task InvalidateCategoriesCacheEntries(string categoryId)
    {
        await _cacheService.RemoveAsync("categories");
        await _cacheService.RemoveAsync($"category-by-id-{categoryId}");
    }
}
