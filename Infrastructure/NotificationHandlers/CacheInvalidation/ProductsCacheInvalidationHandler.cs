using Application.Mediatr.Notifications.Products;
using Infrastructure.Services.Interfaces;
using MediatR;

namespace Infrastructure.NotificationHandlers.CacheInvalidation;

public sealed class ProductsCacheInvalidationHandler :
    INotificationHandler<ProductDeleted>,
    INotificationHandler<ProductUpdated>,
    INotificationHandler<ProductCreated>
{
    private readonly ICacheService _cacheService;

    public ProductsCacheInvalidationHandler(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public async Task Handle(ProductDeleted notification, CancellationToken cancellationToken)
    {
        await InvalidateProductsCacheEntries(notification.Id);
    }

    public async Task Handle(ProductUpdated notification, CancellationToken cancellationToken)
    {
        await InvalidateProductsCacheEntries(notification.Id);
    }

    public async Task Handle(ProductCreated notification, CancellationToken cancellationToken)
    {
        await _cacheService.RemoveAsync("products");
    }

    private async Task InvalidateProductsCacheEntries(string productId)
    {
        await _cacheService.RemoveAsync("products");
        await _cacheService.RemoveAsync($"product-by-id-{productId}");
    }
}
