using System.Linq.Expressions;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Menus;

namespace Nop.Services.Menus;

/// <summary>
/// Menu service interface
/// </summary>
public partial interface IMenuService
{
    #region Menus

    /// <summary>
    /// Deletes a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteMenuAsync(Menu menu);
    
    /// <summary>
    /// Gets all menus
    /// </summary>
    /// <returns>
    /// <param name="showHidden">A value indicating whether to load hidden records</param>
    /// A task that represents the asynchronous operation
    /// The task result contains the menus
    /// </returns>
    Task<IList<Menu>> GetAllMenusAsync(bool showHidden = false);

    /// <summary>
    /// Gets a menu by identifier
    /// </summary>
    /// <param name="menuId">The menu identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a menu
    /// </returns>
    Task<Menu> GetMenuByIdAsync(int menuId);

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
    Task<IList<Menu>> GetMenuByTypeAsync(MenuType menuType, int storeId = 0, bool showHidden = false);

    /// <summary>
    /// Insert a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertMenuAsync(Menu menu);

    /// <summary>
    /// Update a menu
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateMenuAsync(Menu menu);

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
    Task<IPagedList<Menu>> SearchMenusAsync(int storeId = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

    #endregion

    #region Menu items

    /// <summary>
    /// Deletes a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task DeleteMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Gets all menu items by parent identifier
    /// </summary>
    /// <param name="parentMenuItemId">Parent menu item identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    Task<IList<MenuItem>> GetAllMenuItemsByParentIdAsync(int parentMenuItemId, bool showHidden = false);

    /// <summary>
    /// Gets menu items by menu identifier
    /// </summary>
    /// <param name="menuId">Menu identifier</param>
    /// <param name="showHidden">A value indicating whether to show hidden records</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    Task<IList<MenuItem>> GetAllMenuItemsByMenuIdAsync(int menuId, bool showHidden = false);

    /// <summary>
    /// Gets a menu item by identifier
    /// </summary>
    /// <param name="menuItemId">Menu item identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains a menu item
    /// </returns>
    Task<MenuItem> GetMenuItemByIdAsync(int menuItemId);

    /// <summary>
    /// Gets menu items of the given depth
    /// </summary>
    /// <param name="menuId">Menu identifier</param>
    /// <param name="depth">Depth to limit items; pass 0 to get root menu items</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu items
    /// </returns>
    Task<IEnumerable<MenuItem>> GetMenuItemsWithDepthAsync(int menuId, int depth);

    /// <summary>
    /// Insert a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task InsertMenuItemAsync(MenuItem menuItem);

    /// <summary>
    /// Update a menu item
    /// </summary>
    /// <param name="menuItem">Menu item</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task UpdateMenuItemAsync(MenuItem menuItem);

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
    Task<IPagedList<MenuItem>> SearchMenuItemsAsync(int menuId = 0, int pageIndex = 0, int pageSize = int.MaxValue);

    #endregion
}
