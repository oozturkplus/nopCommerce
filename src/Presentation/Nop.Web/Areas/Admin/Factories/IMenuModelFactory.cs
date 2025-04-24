using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Menus;
using Nop.Web.Areas.Admin.Models.Menus;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IMenuModelFactory
{
    #region Menus

    /// <summary>
    /// Prepare menu search model
    /// </summary>
    /// <param name="searchModel">Menu search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu search model
    /// </returns>
    Task<MenuSearchModel> PrepareMenuSearchModelAsync(MenuSearchModel searchModel);

    /// <summary>
    /// Prepare paged menu list model
    /// </summary>
    /// <param name="searchModel">Menu search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu list model
    /// </returns>
    Task<MenuListModel> PrepareMenuListModelAsync(MenuSearchModel searchModel);

    /// <summary>
    /// Prepare menu model
    /// </summary>
    /// <param name="model">Menu model</param>
    /// <param name="menu">Menu</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu model
    /// </returns>
    Task<MenuModel> PrepareMenuModelAsync(MenuModel model, Menu menu, bool excludeProperties = false);

    #endregion

    #region Menu items

    /// <summary>
    /// Prepare paged menu item list model
    /// </summary>
    /// <param name="searchModel">Menu item search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu item list model
    /// </returns>
    Task<MenuItemListModel> PrepareMenuItemListModelAsync(MenuItemSearchModel searchModel);

    /// <summary>
    /// Prepare menu item model
    /// </summary>
    /// <param name="menu">Menu</param>
    /// <param name="model">Menu item model</param>
    /// <param name="menuItem">Menu item</param>
    /// <param name="excludeProperties">Whether to exclude populating of some properties of model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu item model
    /// </returns>
    Task<MenuItemModel> PrepareMenuItemModelAsync(Menu menu, MenuItemModel model, MenuItem menuItem, bool excludeProperties = false);

    #region Select lists

    /// <summary>
    /// Prepare topic list
    /// </summary>
    /// <param name="items">List to add available topics</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    Task PrepareAvailableTopicsAsync(IList<SelectListItem> items);

    /// <summary>
    /// Prepare category list
    /// </summary>
    /// <param name="items">List to add available categories</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    Task PrepareAvailableCategoriesAsync(IList<SelectListItem> items, bool withAllItem = true);

    /// <summary>
    /// Prepare manufacturer list
    /// </summary>
    /// <param name="items">List to add available manufacturers</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    Task PrepareAvailableManufacturersAsync(IList<SelectListItem> items, bool withAllItem = true);

    /// <summary>
    /// Prepare vendor list
    /// </summary>
    /// <param name="items">List to add available vendors</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    Task PrepareAvailableVendorsAsync(IList<SelectListItem> items, bool withAllItem = true);

    #endregion

    #region Products popup

    /// <summary>
    /// Prepare product search model
    /// </summary>
    /// <param name="searchModel">Product search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product search model
    /// </returns>
    Task<SelectMenuItemProductSearchModel> PrepareMenuItemSelectProductSearchModelAsync(SelectMenuItemProductSearchModel searchModel);

    /// <summary>
    /// Prepare product list model
    /// </summary>
    /// <param name="searchModel">Product search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product list model
    /// </returns>
    Task<SelectMenuItemProductListModel> PrepareMenuItemSelectProductListModelAsync(SelectMenuItemProductSearchModel searchModel);

    #endregion

    #endregion
}
