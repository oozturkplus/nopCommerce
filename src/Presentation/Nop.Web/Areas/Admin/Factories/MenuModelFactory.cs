using System.Linq.Expressions;
using System.util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Menus;
using Nop.Core.Http;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Menus;
using Nop.Services.Topics;
using Nop.Services.Vendors;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Menus;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class MenuModelFactory : IMenuModelFactory
{
    #region Fields

    protected readonly CatalogSettings _catalogSettings;
    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
    protected readonly ICategoryService _categoryService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IManufacturerService _manufacturerService;
    protected readonly IMenuService _menuService;
    protected readonly IProductService _productService;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly ITopicService _topicService;
    protected readonly IVendorService _vendorService;
    protected readonly MenuSettings _menuSettings;

    #endregion

    #region Ctor

    public MenuModelFactory(
        CatalogSettings catalogSettings,
        IBaseAdminModelFactory baseAdminModelFactory,
        ICategoryService categoryService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IManufacturerService manufacturerService,
        IMenuService menuService,
        IProductService productService,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        ITopicService topicService,
        IVendorService vendorService,
        MenuSettings menuSettings)
    {
        _catalogSettings = catalogSettings;
        _baseAdminModelFactory = baseAdminModelFactory;
        _categoryService = categoryService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _manufacturerService = manufacturerService;
        _menuService = menuService;
        _productService = productService;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _topicService = topicService;
        _vendorService = vendorService;
        _menuSettings = menuSettings;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Gets title of the menu item
    /// </summary>
    /// <typeparam name="TEntity">Type of localized entity</typeparam>
    /// <param name="menuItem">Menu item</param>
    /// <param name="entity">Entity</param>
    /// <param name="keySelector">Key selector</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the localized title
    /// </returns>
    public virtual async Task<string> GetLocalizedMenuItemTitleAsync<TEntity>(MenuItem menuItem, TEntity entity, Expression<Func<TEntity, string>> keySelector)
        where TEntity : BaseEntity, ILocalizedEntity
    {
        if (entity is null)
            return await _localizationService.GetLocalizedAsync(menuItem, m => m.Title);

        return await _localizationService.GetLocalizedAsync(entity, keySelector);
    }

    /// <summary>
    /// Prepare available menu items
    /// </summary>
    /// <param name="menuItemModel">Menu item model</param>
    /// <param name="depth">Number of menu levels</param>
    /// <param name="menuItemsToAdd">List for adding menu items</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    protected virtual async Task PrepareModelAvailableParentMenuItemsAsync(MenuItemModel menuItemModel, int depth, IList<SelectListItem> menuItemsToAdd)
    {
        var items = await _menuService.GetMenuItemsWithDepthAsync(menuItemModel.MenuId, depth);
        menuItemsToAdd.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.Common.No"), Value = string.Empty });

        foreach (var item in items.Where(item => item.Id != menuItemModel.Id))
        {
            menuItemsToAdd.Add(new SelectListItem { Value = item.Id.ToString(), Text = await GetMenuItemBreadcrumbAsync(item, items) });
        }
    }

    /// <summary>
    /// Get breadcrumb for menu item
    /// </summary>
    /// <param name="item">Menu item</param>
    /// <param name="allItems">All menu items</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains menu item breadcrumb 
    /// </returns>
    protected virtual async Task<string> GetMenuItemBreadcrumbAsync(MenuItem item, IEnumerable<MenuItem> allItems)
    {
        var titles = new List<string>();
        var current = item;

        while (current is not null)
        {
            titles.Insert(0, current.MenuItemType switch
            {
                MenuItemType.Vendor => await GetLocalizedMenuItemTitleAsync(current, await _vendorService.GetVendorByIdAsync(current.EntityId ?? 0), v => v.Name),
                MenuItemType.Category => await GetLocalizedMenuItemTitleAsync(current, await _categoryService.GetCategoryByIdAsync(current.EntityId ?? 0), v => v.Name),
                MenuItemType.TopicPage => await GetLocalizedMenuItemTitleAsync(current, await _topicService.GetTopicByIdAsync(current.EntityId ?? 0), v => v.Title),
                MenuItemType.Product => await GetLocalizedMenuItemTitleAsync(current, await _productService.GetProductByIdAsync(current.EntityId ?? 0), v => v.Name),
                MenuItemType.Manufacturer => await GetLocalizedMenuItemTitleAsync(current, await _manufacturerService.GetManufacturerByIdAsync(current.EntityId ?? 0), v => v.Name),
                _ => await _localizationService.GetLocalizedAsync(current, m => m.Title)
            });
            current = allItems.FirstOrDefault(x => x.Id == current.ParentId);
        }

        return string.Join(" >> ", titles);
    }

    #endregion

    #region Methods

    #region Menus

    /// <summary>
    /// Prepare menu search model
    /// </summary>
    /// <param name="searchModel">Menu search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu search model
    /// </returns>
    public virtual Task<MenuSearchModel> PrepareMenuSearchModelAsync(MenuSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    /// <summary>
    /// Prepare paged menu list model
    /// </summary>
    /// <param name="searchModel">Menu search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the menu list model
    /// </returns>
    public virtual async Task<MenuListModel> PrepareMenuListModelAsync(MenuSearchModel searchModel)
    {
        var menus = await _menuService.SearchMenusAsync(pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize, showHidden: true);

        //prepare list model
        return await new MenuListModel().PrepareToGridAsync(searchModel, menus, () =>
        {
            //fill in model values from the entity
            return menus.SelectAwait(async menu =>
            {
                var model = menu.ToModel<MenuModel>();
                model.MenuTypeName = await _localizationService.GetLocalizedEnumAsync((MenuType)model.MenuTypeId);

                return model;
            });
        });
    }

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
    public virtual async Task<MenuModel> PrepareMenuModelAsync(MenuModel model, Menu menu, bool excludeProperties = false)
    {
        Func<MenuLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (menu != null)
        {
            if (model == null)
                model = menu.ToModel<MenuModel>();

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(menu, entity => entity.Name, languageId, false, false);
            };

            model.MenuItemSearchModel.MenuId = menu.Id;
        }

        //prepare model stores
        await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, menu, excludeProperties);
        model.AvailableMenuTypes = (await MenuType.Footer.ToSelectListAsync(false)).ToList();

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

        return model;
    }

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
    public virtual async Task<MenuItemListModel> PrepareMenuItemListModelAsync(MenuItemSearchModel searchModel)
    {
        var menuItems = await _menuService.SearchMenuItemsAsync(searchModel.MenuId, searchModel.Page - 1, searchModel.PageSize);

        //prepare list model
        return await new MenuItemListModel().PrepareToGridAsync(searchModel, menuItems, () =>
        {
            //fill in model values from the entity
            return menuItems.SelectAwait(async menuItem =>
            {
                var model = menuItem.ToModel<MenuItemModel>();

                model.Breadcrumb = await GetMenuItemBreadcrumbAsync(menuItem, menuItems);
                model.MenuItemTypeName = await _localizationService.GetLocalizedEnumAsync((MenuItemType)model.MenuItemTypeId);

                return model;
            });
        });
    }

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
    public virtual async Task<MenuItemModel> PrepareMenuItemModelAsync(Menu menu, MenuItemModel model, MenuItem menuItem, bool excludeProperties = false)
    {
        ArgumentNullException.ThrowIfNull(menu);
        Func<MenuItemLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (menuItem != null)
        {
            if (model == null)
                model = menuItem.ToModel<MenuItemModel>();

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Title = await _localizationService.GetLocalizedAsync(menuItem, entity => entity.Title, languageId, false, false);
            };

            switch (menuItem.MenuItemType)
            {
                case MenuItemType.Product:
                    var product = await _productService.GetProductByIdAsync(menuItem.EntityId ?? 0);
                    model.ProductName = product?.Name;
                    model.ProductId = product?.Id;
                    break;
                case MenuItemType.TopicPage:
                    model.TopicId = menuItem.EntityId ?? 0;
                    break;
                case MenuItemType.Category:
                    model.CategoryId = menuItem.EntityId ?? 0;
                    break;
                case MenuItemType.Vendor:
                    model.VendorId = menuItem.EntityId ?? 0;
                    break;
                case MenuItemType.Manufacturer:
                    model.ManufacturerId = menuItem.EntityId ?? 0;
                    break;
                default:
                    break;
            }
        }
        else
        {
            model.MenuId = menu.Id;
            model.Published = true;
            model.MaximumNumberEntities = _menuSettings.MaximumNumberEntities;
            model.NumberOfSubItemsPerGridElement = _menuSettings.NumberOfSubItemsPerGridElement;
            model.NumberOfItemsPerGridRow = _menuSettings.NumberOfItemsPerGridRow;
        }

        var isMainMenu = menu.MenuType == MenuType.Main;

        await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, menuItem, excludeProperties);

        model.AvailableMenuItemTypes = (await MenuItemType.StandardPage.ToSelectListAsync(false)).ToList();

        await _baseAdminModelFactory.ConstantsToSelectListAsync(model.AvailableStandardRoutes, typeof(NopStandardRouteNames), sortItems: true);
        model.AvailableStandardRoutes = model.AvailableStandardRoutes.OrderBy(x => x.Text).ToList();

        await PrepareAvailableMenuItemTemplatesAsync(model.AvailableMenuItemTemplates, menu.MenuType);
        await PrepareAvailableCategoriesAsync(model.AvailableCategories, isMainMenu);
        await PrepareAvailableTopicsAsync(model.AvailableTopics);
        await PrepareAvailableManufacturersAsync(model.AvailableManufacturers, isMainMenu);
        await PrepareAvailableVendorsAsync(model.AvailableVendors, isMainMenu);

        if (isMainMenu)
            await PrepareModelAvailableParentMenuItemsAsync(model, _menuSettings.MaximumMainMenuLevels, model.AvailableMenuItems);

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

        return model;
    }

    #region Select lists

    /// <summary>
    /// Prepare menu item template list
    /// </summary>
    /// <param name="items">List to add available categories</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    public virtual async Task PrepareAvailableMenuItemTemplatesAsync(IList<SelectListItem> items, MenuType menuType)
    {
        ArgumentNullException.ThrowIfNull(items);

        items.AddRange(menuType switch
        {
            MenuType.Footer => await MenuItemTemplate.Simple.ToSelectListAsync(false, valuesToExclude: new[] { (int)MenuItemTemplate.Grid, (int)MenuItemTemplate.List }),
            _ => await MenuItemTemplate.Simple.ToSelectListAsync(false)
        });
    }

    /// <summary>
    /// Prepare topic list
    /// </summary>
    /// <param name="items">List to add available topics</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    public virtual async Task PrepareAvailableTopicsAsync(IList<SelectListItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        //get topics
        var topics = await _topicService.GetAllTopicsAsync(storeId: 0, showHidden: true);

        var availableTopics = topics.Select(t => new SelectListItem
        {
            Text = !string.IsNullOrEmpty(t.SystemName) ? t.SystemName : t.Title,
            Value = t.Id.ToString()
        }).ToList();

        items.AddRange(availableTopics);
    }

    /// <summary>
    /// Prepare category list
    /// </summary>
    /// <param name="items">List to add available categories</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    public virtual async Task PrepareAvailableCategoriesAsync(IList<SelectListItem> items, bool withAllItem = true)
    {
        ArgumentNullException.ThrowIfNull(items);

        //get categories
        var categories = await _categoryService.GetAllCategoriesAsync(showHidden: true);

        var availableCategories = await categories.SelectAwait(async category => new SelectListItem
        {
            Text = await _localizationService.GetLocalizedAsync(category, c => c.Name),
            Value = category.Id.ToString()
        }).ToListAsync();

        if (withAllItem)
            items.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.ContentManagement.Menus.MenuItem.Fields.Category.All"), Value = "0" });

        items.AddRange(availableCategories);
    }

    /// <summary>
    /// Prepare manufacturer list
    /// </summary>
    /// <param name="items">List to add available manufacturers</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    public virtual async Task PrepareAvailableManufacturersAsync(IList<SelectListItem> items, bool withAllItem = true)
    {
        ArgumentNullException.ThrowIfNull(items);

        //get manufacturers
        var manufacturers = await _manufacturerService.GetAllManufacturersAsync(showHidden: true);

        var availableManufacturers = await manufacturers.SelectAwait(async manufacturer => new SelectListItem
        {
            Text = await _localizationService.GetLocalizedAsync(manufacturer, m => m.Name),
            Value = manufacturer.Id.ToString()
        }).ToListAsync();

        if (withAllItem)
            items.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.ContentManagement.Menus.MenuItem.Fields.Manufacturer.All"), Value = "0" });

        items.AddRange(availableManufacturers);
    }

    /// <summary>
    /// Prepare vendor list
    /// </summary>
    /// <param name="items">List to add available vendors</param>
    /// <param name="withAllItem">Whether to insert the "All" item</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// </returns>
    public virtual async Task PrepareAvailableVendorsAsync(IList<SelectListItem> items, bool withAllItem = true)
    {
        ArgumentNullException.ThrowIfNull(items);

        //get vendors
        var vendors = await _vendorService.GetAllVendorsAsync(showHidden: true);

        var availableVendors = await vendors.SelectAwait(async vendor => new SelectListItem
        {
            Text = await _localizationService.GetLocalizedAsync(vendor, m => m.Name),
            Value = vendor.Id.ToString()
        }).ToListAsync();

        if (withAllItem)
            items.Add(new SelectListItem { Text = await _localizationService.GetResourceAsync("Admin.ContentManagement.Menus.MenuItem.Fields.Vendor.All"), Value = "0" });

        items.AddRange(availableVendors);
    }

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
    public virtual async Task<SelectMenuItemProductSearchModel> PrepareMenuItemSelectProductSearchModelAsync(SelectMenuItemProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare available stores
        await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

        searchModel.HideStoresList = _catalogSettings.IgnoreStoreLimitations || searchModel.AvailableStores.SelectionIsNotPossible();

        //prepare page parameters
        searchModel.SetGridPageSize();

        return searchModel;
    }

    /// <summary>
    /// Prepare product list model
    /// </summary>
    /// <param name="searchModel">Product search model</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the product list model
    /// </returns>
    public virtual async Task<SelectMenuItemProductListModel> PrepareMenuItemSelectProductListModelAsync(SelectMenuItemProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get categories
        var products = await _productService.SearchProductsAsync(keywords: searchModel.SearchKeywords,
            showHidden: true,
            storeId: searchModel.SearchStoreId,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        //prepare grid model
        return new SelectMenuItemProductListModel().PrepareToGrid(searchModel, products, () =>
        {
            return products.Select(product => new SelectMenuItemProductModel
            {
                Id = product.Id,
                Name = product.Name,
                Published = product.Published
            });
        });
    }

    #endregion

    #endregion

    #endregion
}
