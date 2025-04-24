using Nop.Core.Domain.Menus;
using Nop.Services.Caching;

namespace Nop.Services.Menus.Caching;

/// <summary>
/// Represents a menu item cache event consumer
/// </summary>
public partial class MenuCacheEventConsumer : CacheEventConsumer<Menu>
{
    /// <summary>
    /// Clear cache data
    /// </summary>
    /// <param name="entity">Entity</param>
    /// <param name="entityEventType">Entity event type</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    protected override async Task ClearCacheAsync(Menu entity, EntityEventType entityEventType)
    {
        await base.ClearCacheAsync(entity, entityEventType);
    }
}
