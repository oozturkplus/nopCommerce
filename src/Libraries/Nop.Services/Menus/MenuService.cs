using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Menus;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;

namespace Nop.Services.Menus;

/// <summary>
/// Menu service
/// </summary>
public partial class MenuService : IMenuService
{
    #region Fields

    protected readonly IAclService _aclService;
    protected readonly ICustomerService _customerService;
    protected readonly ILocalizationService _localizationService;
    protected readonly IRepository<Menu> _menuRepository;
    protected readonly IRepository<MenuItem> _menuItemRepository;
    protected readonly IStaticCacheManager _staticCacheManager;
    protected readonly IStoreContext _storeContext;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public MenuService(
        IAclService aclService,
        ICustomerService customerService,
        ILocalizationService localizationService,
        IRepository<Menu> menuRepository,
        IRepository<MenuItem> menuItemRepository,
        IStaticCacheManager staticCacheManager,
        IStoreContext storeContext,
        IStoreMappingService storeMappingService,
        IWorkContext workContext)
    {
        _aclService = aclService;
        _customerService = customerService;
        _localizationService = localizationService;
        _menuRepository = menuRepository;
        _menuItemRepository = menuItemRepository;
        _staticCacheManager = staticCacheManager;
        _storeContext = storeContext;
        _storeMappingService = storeMappingService;
        _workContext = workContext;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Sort menu items for tree representation
    /// </summary>
    /// <param name="menuItemsByParentId">Menu items for sort</param>
    /// <param name="parentId">Parent menu item identifier</param>
    /// <param name="ignoreMenuItemWithoutExistingParent">A value indicating whether menu items without parent menu item in provided list (source) should be ignored</param>
    /// <returns>
    /// An enumerable containing the sorted menu items
    /// </returns>
    protected virtual IEnumerable<MenuItem> SortMenuItemsForTree(
        ILookup<int, MenuItem> menuItemsByParentId,
        int parentId = 0,
        bool ignoreMenuItemWithoutExistingParent = false)
    {
        ArgumentNullException.ThrowIfNull(menuItemsByParentId);

        var remaining = parentId > 0
            ? new HashSet<int>(0)
            : menuItemsByParentId.Select(g => g.Key).ToHashSet();
        remaining.Remove(parentId);

        foreach (var item in menuItemsByParentId[parentId].OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id))
        {
            yield return item;

            remaining.Remove(item.Id);

            foreach (var subItem in SortMenuItemsForTree(menuItemsByParentId, item.Id, true))
            {
                yield return subItem;
                remaining.Remove(subItem.Id);
            }
        }

        if (ignoreMenuItemWithoutExistingParent)
            yield break;

        //find menu items without parent in provided menu item source and return them
        var orphans = remaining
            .SelectMany(id => menuItemsByParentId[id])
            .OrderBy(c => c.ParentId)
            .ThenBy(c => c.DisplayOrder)
            .ThenBy(c => c.Id);

        foreach (var orphan in orphans)
            yield return orphan;
    }

    #endregion

    #region Methods

    #region Menus

    /// <summary>
    /// Deletes a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task DeleteMenuAsync(Menu menu)
    {
        return _menuRepository.DeleteAsync(menu);
    }

    /// <summary>
    /// Gets all menus
    /// </summary>
    /// <returns>
    /// <param name="showHidden">A value indicating whether to load hidden records</param>
    /// A task that represents the asynchronous operation
    /// The task result contains the menus
    /// </returns>
    public virtual async Task<IList<Menu>> GetAllMenusAsync(bool showHidden = false)
    {
        var menus = await _menuRepository.GetAllAsync(query => query, getCacheKey: _ => default, includeDeleted: false);

        if (showHidden)
            return menus;

        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        var currentStore = await _storeContext.GetCurrentStoreAsync();

        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopMenuDefaults.MenuByTypeCacheKey,
            currentStore, await _customerService.GetCustomerRoleIdsAsync(currentCustomer));

        return await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await menus
                .WhereAwait(async m => m.Published && await _aclService.AuthorizeAsync(m, currentCustomer) && await _storeMappingService.AuthorizeAsync(m, currentStore.Id))
                .ToListAsync();
        });
    }

    /// <summary>
    /// Gets a menu by identifier
    /// </summary>
    /// <param name="menuId">The menu identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a menu
    /// </returns>
    public virtual Task<Menu> GetMenuByIdAsync(int menuId)
    {
        return _menuRepository.GetByIdAsync(menuId, cache => default, includeDeleted: false);
    }

    /// <summary>
    /// Gets a menu by menu type
    /// </summary>
    /// <param name="menuType">Menu type</param>
    /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
    /// <param name="showHidden">A value indicating whether to load hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a list of the menu
    /// </returns>
    public virtual async Task<IList<Menu>> GetMenuByTypeAsync(MenuType menuType, int storeId = 0, bool showHidden = false)
    {
        var allMenus = await SearchMenusAsync(storeId: storeId, showHidden: showHidden);
        return allMenus.Where(m => m.MenuTypeId == (int)menuType).ToList();
    }

    /// <summary>
    /// Insert a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task InsertMenuAsync(Menu menu)
    {
        return _menuRepository.InsertAsync(menu);
    }

    /// <summary>
    /// Update a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task UpdateMenuAsync(Menu menu)
    {
        return _menuRepository.UpdateAsync(menu);
    }

    /// <summary>
    /// Search menus
    /// </summary>
    /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menus
    /// </returns>
    public virtual async Task<IPagedList<Menu>> SearchMenusAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
    {
        return await _menuRepository.GetAllPagedAsync(async query =>
        {
            if (!showHidden)
            {
                query = query.Where(m => m.Published);

                //apply ACL constraints
                var customer = await _workContext.GetCurrentCustomerAsync();
                query = await _aclService.ApplyAcl(query, customer);
            }

            if (!showHidden || storeId > 0)
            {
                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);
            }

            return query
                .OrderBy(m => m.MenuTypeId)
                .ThenBy(m => m.DisplayOrder)
                .ThenBy(m => m.Id);
        }, pageIndex, pageSize, includeDeleted: false);
    }

    #endregion

    #region Menu items

    /// <summary>
    /// Deletes a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task DeleteMenuItemAsync(MenuItem menuItem)
    {
        await _menuItemRepository.DeleteAsync(menuItem);

        foreach (var subitem in await GetAllMenuItemsByParentIdAsync(menuItem.Id, true))
        {
            subitem.ParentId = 0;
            await UpdateMenuItemAsync(subitem);
        }
    }

    /// <summary>
    /// Gets all menu items by parent identifier
    /// </summary>
    /// <param name="parentMenuItemId">Parent menu item identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    public virtual async Task<IList<MenuItem>> GetAllMenuItemsByParentIdAsync(int parentMenuItemId, bool showHidden = false)
    {
        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

        return await _menuItemRepository.GetAllAsync(async query =>
        {
            if (!showHidden)
            {
                query = query.Where(mi => mi.Published);

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, store.Id);

                //apply ACL constraints
                query = await _aclService.ApplyAcl(query, customerRoleIds);
            }

            return query.Where(mi => mi.ParentId == parentMenuItemId)
                .OrderBy(mi => mi.DisplayOrder).ThenBy(c => c.Id);
        }, cache => cache.PrepareKeyForDefaultCache(NopMenuDefaults.MenuItemsByParentCacheKey, parentMenuItemId, showHidden, customerRoleIds, store));
    }

    /// <summary>
    /// Gets menu items by menu identifier
    /// </summary>
    /// <param name="menuId">Menu identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    public virtual async Task<IList<MenuItem>> GetAllMenuItemsByMenuIdAsync(int menuId, bool showHidden = false)
    {
        ArgumentOutOfRangeException.ThrowIfZero(menuId);

        var store = await _storeContext.GetCurrentStoreAsync();
        var customer = await _workContext.GetCurrentCustomerAsync();
        var customerRoleIds = await _customerService.GetCustomerRoleIdsAsync(customer);

        return await _menuItemRepository.GetAllAsync(async query =>
        {
            if (!showHidden)
            {
                query = query.Where(mi => mi.Published);

                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, store.Id);

                //apply ACL constraints
                query = await _aclService.ApplyAcl(query, customerRoleIds);
            }

            return query.Where(mi => mi.MenuId == menuId).OrderBy(mi => mi.DisplayOrder).ThenBy(mi => mi.Id);
        }, cache => cache.PrepareKeyForDefaultCache(NopMenuDefaults.MenuItemsByMenuCacheKey, menuId, showHidden, customerRoleIds, store));
    }

    /// <summary>
    /// Gets a menu item by identifier
    /// </summary>
    /// <param name="menuItemId">The menu item identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a menu item
    /// </returns>
    public virtual Task<MenuItem> GetMenuItemByIdAsync(int menuItemId)
    {
        return _menuItemRepository.GetByIdAsync(menuItemId, cache => default);
    }

    /// <summary>
    /// Gets menu items of the given depth
    /// </summary>
    /// <param name="menuId">Menu identifier</param>
    /// <param name="depth">Depth to limit items; pass 0 to get root menu items</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    public virtual async Task<IEnumerable<MenuItem>> GetMenuItemsWithDepthAsync(int menuId, int depth)
    {
        ArgumentOutOfRangeException.ThrowIfZero(menuId);

        var allItems = await GetAllMenuItemsByMenuIdAsync(menuId);

        return getChildren(allItems.Where(item => item.ParentId == 0).OrderBy(item => item.DisplayOrder), depth);

        IEnumerable<MenuItem> getChildren(IEnumerable<MenuItem> menuItems, int depthLimit)
        {
            if (depthLimit == 0)
                yield break;

            depthLimit--;

            foreach (var item in menuItems)
            {
                yield return item;

                foreach (var child in getChildren(allItems.Where(x => x.ParentId == item.Id), depthLimit).OrderBy(item => item.DisplayOrder))
                    yield return child;
            }
        }
    }

    /// <summary>
    /// Insert a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual Task InsertMenuItemAsync(MenuItem menuItem)
    {
        return _menuItemRepository.InsertAsync(menuItem);
    }

    /// <summary>
    /// Update a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public virtual async Task UpdateMenuItemAsync(MenuItem menuItem)
    {
        ArgumentNullException.ThrowIfNull(menuItem);

        //validate hierarchy
        var parentMenuItem = await GetMenuItemByIdAsync(menuItem.ParentId);
        while (parentMenuItem != null)
        {
            if (menuItem.Id == parentMenuItem.Id)
            {
                menuItem.ParentId = 0;
                break;
            }

            parentMenuItem = await GetMenuItemByIdAsync(parentMenuItem.ParentId);
        }

        await _menuItemRepository.UpdateAsync(menuItem);
    }

    /// <summary>
    /// Search menu items
    /// </summary>
    /// <param name="menuId">Menu identifier; 0 or null to load all records</param>
    /// <param name="pageIndex">Page index</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    public virtual async Task<IPagedList<MenuItem>> SearchMenuItemsAsync(
        int menuId = 0,
        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var unsortedMenuItems = await _menuItemRepository.GetAllPagedAsync(query =>
        {
            return query.Where(menuItem => menuItem.MenuId == menuId)
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.Id);
        });

        var sortedMenuItems = SortMenuItemsForTree(unsortedMenuItems.ToLookup(c => c.ParentId))
            .ToList();

        //paging
        return new PagedList<MenuItem>(sortedMenuItems, pageIndex, pageSize);
    }

    #endregion

    #endregion
}
