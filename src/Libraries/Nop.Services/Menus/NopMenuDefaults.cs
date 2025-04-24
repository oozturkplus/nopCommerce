using Nop.Core.Caching;

namespace Nop.Services.Menus;
public static partial class NopMenuDefaults
{
    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : menu ID
    /// {1} : show hidden records?
    /// {2} : roles of the current user
    /// {3} : store ID
    /// </remarks>
    public static CacheKey MenuItemsByMenuCacheKey => new("Nop.menu.menuitems.bymenu.{0}-{1}-{2}-{3}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : parent menu item ID
    /// {1} : show hidden records?
    /// {2} : roles of the current user
    /// {3} : store ID
    /// </remarks>
    public static CacheKey MenuItemsByParentCacheKey => new("Nop.menu.menuitems.byparent.{0}-{1}-{2}-{3}");

    /// <summary>
    /// Gets a key pattern to clear cache
    /// </summary>
    /// <remarks>
    /// {0} : menu ID
    /// </remarks>
    public static string MenuItemsByMenuPrefix => "Nop.menu.menuitems.bymenu.{0}";

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : current store ID
    /// {1} : roles of the current user
    /// </remarks>
    public static CacheKey MenuByTypeCacheKey => new("Nop.menu.bytype.{0}-{1}");
    public static string MenuPrefix => "Nop.menu.";
}
