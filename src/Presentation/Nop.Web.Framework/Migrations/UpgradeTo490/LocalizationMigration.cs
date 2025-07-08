using FluentMigrator;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Localization;
using Nop.Web.Framework.Extensions;

namespace Nop.Web.Framework.Migrations.UpgradeTo490;

[NopUpdateMigration("2024-12-01 00:00:00", "4.90", UpdateMigrationType.Localization)]
public class LocalizationMigration : MigrationBase
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        //do not use DI, because it produces exception on the installation process
        var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

        var (languageId, _) = this.GetLanguageData();

        #region Delete locales

        localizationService.DeleteLocaleResources(new List<string>
        {
            //#7569
            "Admin.Configuration.AppSettings.Common.PluginStaticFileExtensionsBlacklist",
            "Admin.Configuration.AppSettings.Common.PluginStaticFileExtensionsBlacklist.Hint",
            //#7590
            "Checkout.RedirectMessage",
            
            //#1779
            "ActivityLog.PublicStore.Login",

            //#7390
            "Footer.Information",
            "Footer.MyAccount",
            "Footer.CustomerService",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn1",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn1.Hint",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn2",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn2.Hint",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn3",
            "Admin.ContentManagement.Topics.Fields.IncludeInFooterColumn3.Hint",
            "Admin.ContentManagement.Topics.Fields.IncludeInTopMenu",
            "Admin.ContentManagement.Topics.Fields.IncludeInTopMenu.Hint",
            "Admin.ConfigurationSteps.TopicList.Location.Title",
            "Admin.ConfigurationSteps.TopicList.Location.Text",
            "Admin.Configuration.Settings.GeneralCommon.BlockTitle.FooterItems",
            "Admin.Configuration.Settings.GeneralCommon.BlockTitle.TopMenuItems",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayApplyVendorAccountFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayApplyVendorAccountFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayBlogFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayBlogFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCompareProductsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCompareProductsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayContactUsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayContactUsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerAddressesFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerAddressesFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerInfoFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerInfoFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerOrdersFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayCustomerOrdersFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayForumsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayForumsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewProductsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewProductsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayNewsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayProductSearchFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayProductSearchFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayRecentlyViewedProductsFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayRecentlyViewedProductsFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayShoppingCartFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayShoppingCartFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplaySitemapFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplaySitemapFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayWishlistFooterItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultFooterItemSettingsModel.DisplayWishlistFooterItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayBlogMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayContactUsMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayCustomerInfoMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayForumsMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayHomepageMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayHomepageMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayNewProductsMenuItem.Hint",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem",
            "Admin.Configuration.Settings.GeneralCommon.DisplayDefaultMenuItemSettings.DisplayProductSearchMenuItem.Hint",
            "MainMenu.AjaxFailure",
            "Admin.Catalog.Categories.Fields.IncludeInTopMenu",
            "Admin.Catalog.Categories.Fields.IncludeInTopMenu.Hint",
        });

        #endregion

        #region Rename locales

        #endregion

        #region Add or update locales

        localizationService.AddOrUpdateLocaleResource(new Dictionary<string, string>
        {
            //#4834
            ["Admin.Configuration.Settings.GeneralCommon.AdminArea.UseStickyHeaderLayout"] = "Use sticky header",
            ["Admin.Configuration.Settings.GeneralCommon.AdminArea.UseStickyHeaderLayout.Hint"] = "The content header (containing the action buttons) will stick to the top when you reach its scroll position.",

            //#3425
            ["Admin.ContentManagement.MessageTemplates.Description.OrderCompleted.StoreOwnerNotification"] = "This message template is used to notify a store owner that the certain order was completed. The order gets the order status Complete when it's paid and delivered, or it can be changed manually to Complete in Sales - Orders.",

            //#7387
            ["Admin.Catalog.Products.Fields.AgeVerification"] = "Age verification",
            ["Admin.Catalog.Products.Fields.AgeVerification.Hint"] = "Check to require customer registration with date of birth before placing an order.",
            ["Admin.Catalog.Products.Fields.AgeVerification.DateOfBirthDisabled"] = "It looks like you have <a href=\"{0}\" target=\"_blank\">Date of Birth</a> setting disabled.",
            ["Admin.Catalog.Products.Fields.MinimumAgeToPurchase"] = "Minimum age to purchase",
            ["Admin.Catalog.Products.Fields.MinimumAgeToPurchase.Hint"] = "Enter the minimum age for purchasing this product.",
            ["Admin.Catalog.Products.Fields.MinimumAgeToPurchase.ShouldBeGreaterThanZero"] = "The minimum age for purchasing should be greater 0",
            ["ShoppingCart.DateOfBirthRequired"] = "This product has age restrictions. Please specify your age in the account details",
            ["ShoppingCart.MinimumAgeToPurchase"] = "This product is available to customers who are {0} years of age or older",
            ["Admin.Configuration.Settings.ProductEditor.AgeVerification"] = "Age verification",

            //#2184
            ["Admin.Catalog.Products.Multimedia.Pictures.Alert.VendorNumberPicturesLimit"] = "The maximum number of product pictures has been reached.",

            //#7398
            ["Admin.ConfigurationSteps.Product.Details.Text"] = "Enter the relevant product details in these fields. The screenshot below shows how they will be displayed on the product page with the default nopCommerce theme: <div class=\"row row-cols-1\"><img class=\"img-thumbnail mt-3\" src=\"/js/admintour/images/product-page.jpg\"/></div>",
            ["Admin.ConfigurationSteps.PaymentPayPal.ApiCredentials.Text"] = "If you already have an app created in your PayPal account, follow these steps.",

            //#5345
            ["Admin.ContentManagement.Topics.Fields.AvailableEndDateTime"] = "Availability end date",
            ["Admin.ContentManagement.Topics.Fields.AvailableEndDateTime.Hint"] = "The end of the topic's availability (UTC).",
            ["Admin.ContentManagement.Topics.Fields.AvailableEndDateTime.GreaterThanOrEqualToStartDate"] = "The end date must be greater than or equal to the start date.",
            ["Admin.ContentManagement.Topics.Fields.AvailableStartDateTime"] = "Availability start date",
            ["Admin.ContentManagement.Topics.Fields.AvailableStartDateTime.Hint"] = "The start of the topic's availability (UTC).",

            //#6407
            ["ActivityLog.PublicStore.PasswordChanged"] = "Public store. Customer has changed the password",

            //#7498
            ["Admin.Configuration.AppSettings.Common.PermitLimit.Hint"] = "Maximum number of permit counters that can be allowed in a window (1 minute). Must be set to a value > 0 by the time these options are passed to the constructor of FixedWindowRateLimiter. If set to 0 then the limitation is off.",
            ["Admin.Configuration.AppSettings.Common.QueueCount.Hint"] = "Maximum cumulative permit count of queued acquisition requests. Must be set to a value >= 0 by the time these options are passed to the constructor of FixedWindowRateLimiter. If set to 0 then the Queue is off.",

            //#4170
            ["Admin.Promotions.Campaigns.Copy.Name"] = "New campaign name",
            ["Admin.Promotions.Campaigns.Copy.Name.Hint"] = "The name of the new campaign.",
            ["Admin.Promotions.Campaigns.Copy.Name.New"] = "{0} - copy",
            ["Admin.Promotions.Campaigns.Copy"] = "Copy campaign",
            ["Admin.Promotions.Campaigns.Copied"] = "The campaign has been copied successfully",

            //#7477
            ["Pdf.Order"] = "Order #{0}",
            ["Pdf.Shipment"] = "Shipment #{0}",

            //#5279
            ["Search.SearchInTags"] = "Search in product tags",

            //#6407
            ["Admin.Catalog.ProductTags.Info"] = "Product tag info",
            ["Admin.Catalog.ProductTags.Seo"] = "SEO",
            ["Admin.Catalog.ProductTags.Fields.MetaKeywords"] = "Meta keywords",
            ["Admin.Catalog.ProductTags.Fields.MetaKeywords.Hint"] = "Meta keywords to be added to product tag page header.",
            ["Admin.Catalog.ProductTags.Fields.MetaDescription"] = "Meta description",
            ["Admin.Catalog.ProductTags.Fields.MetaDescription.Hint"] = "Meta description to be added to product tag page header.",
            ["Admin.Catalog.ProductTags.Fields.MetaTitle"] = "Meta title",
            ["Admin.Catalog.ProductTags.Fields.MetaTitle.Hint"] = "Override the page title. The default is the name of the product tag.",

            //#7571
            ["Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnCheckGiftCardBalance"] = "Show on check gift card balance page",
            ["Admin.Configuration.Settings.GeneralCommon.CaptchaShowOnCheckGiftCardBalance.Hint"] = "Check to show CAPTCHA on check gift card balance page.",

            //#5771
            ["Admin.Catalog.ProductTags.TaggedProducts"] = "Used by products",
            ["Admin.Catalog.ProductTags.TaggedProducts.Product"] = "Product",
            ["Admin.Catalog.ProductTags.TaggedProducts.Published"] = "Published",

            //#7405
            ["Admin.Configuration.Settings.Catalog.ExportImportCategoryUseLimitedToStores"] = "Export / Import categories with \"limited to stores\"",
            ["Admin.Configuration.Settings.Catalog.ExportImportCategoryUseLimitedToStores.Hint"] = "Check if categories should be exported / imported with \"limited to stores\" property.",

            //#820
            ["Currency.Selector.Text.Pattern"] = "{0}, {1}",

            //#5652
            ["Admin.System.SystemInfo.DatabaseCollation"] = "Database collation",
            ["Admin.System.SystemInfo.DatabaseCollation.Hint"] = "The collation defines the rules for sorting and comparing data.",

            //#1779
            ["ActivityLog.PublicStore.Login.Fail"] = "Public store. Customer has failed to log in: {0}",
            ["Admin.Configuration.Settings.CustomerUser.NotifyFailedLoginAttempt"] = "Notify customers about failed login attempts",
            ["Admin.Configuration.Settings.CustomerUser.NotifyFailedLoginAttempt.Hint"] = "Check to enable customer notifications on failed login attempts.",
            ["ActivityLog.PublicStore.Login.Success"] = "Public store. Customer has logged in",

            //2921
            ["Admin.System.Maintenance.ShrinkDatabase"] = "Shrink database",
            ["Admin.System.Maintenance.ShrinkDatabase.Complete"] = "Database shrinking completed",
            ["Admin.System.Maintenance.ShrinkDatabase.Progress"] = "Processing...",
            ["Admin.System.Maintenance.ShrinkDatabase.Text"] = "Reclaim disk space by reorganizing physical data storage",

            //#7515
            ["Admin.Catalog.Attributes.ProductAttributes.List.SearchProductAttributeName"] = "Product attribute name",
            ["Admin.Catalog.Attributes.ProductAttributes.List.SearchProductAttributeName.Hint"] = "A product attribute name.",

            //#1266
            ["Account.CustomerOrders.Period"] = "Orders from",
            ["Account.CustomerRecurringPayments"] = "Recurring payments",
            ["Account.CustomerRecurringPayments.NoPayments"] = "No payments",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.All"] = "all time",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.Day"] = "the past day",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.Week"] = "the past week",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.Month"] = "the past month",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.HalfYear"] = "the past six months",
            ["Enums.Nop.Web.Models.Order.OrderHistoryPeriods.Year"] = "the past year",

            //#7630
            ["Admin.Configuration.Settings.Tax.HmrcApiUrl"] = "HMRC API URL",
            ["Admin.Configuration.Settings.Tax.HmrcApiUrl.Hint"] = "The base HMRC access API URL.",
            ["Admin.Configuration.Settings.Tax.HmrcClientId"] = "HMRC API client ID",
            ["Admin.Configuration.Settings.Tax.HmrcClientId.Hint"] = "Your HMRC API client ID is a unique identifier which created when you added your application.",
            ["Admin.Configuration.Settings.Tax.HmrcClientSecret"] = "HMRC API client secret",
            ["Admin.Configuration.Settings.Tax.HmrcClientSecret.Hint"] = "Your client secret is a unique passphrase that you generate to authorise your application.",

            //#7390
            ["ActivityLog.AddNewMenu"] = "Added a new menu ('{0}')",
            ["ActivityLog.AddNewMenuItem"] = "Added a new menu item ('{0}')",
            ["ActivityLog.DeleteMenu"] = "Deleted a menu ('{0}')",
            ["ActivityLog.DeleteMenuItem"] = "Deleted a menu item ('{0}')",
            ["ActivityLog.EditMenu"] = "Edited a menu ('{0}')",
            ["ActivityLog.EditMenuItem"] = "Edited a menu item ('{0}')",
            ["Admin.ContentManagement.Menus"] = "Menus",
            ["Admin.ContentManagement.Menus.Added"] = "The new menu has been added successfully.",
            ["Admin.ContentManagement.Menus.AddNew"] = "Add a new menu",
            ["Admin.ContentManagement.Menus.BackToList"] = "back to menu list",
            ["Admin.ContentManagement.Menus.Deleted"] = "The menu has been deleted successfully.",
            ["Admin.ContentManagement.Menus.EditMenuDetails"] = "Edit menu details",
            ["Admin.ContentManagement.Menus.Info"] = "Info",
            ["Admin.ContentManagement.Menus.Fields.DisplayAllCategories"] = "Display all categories",
            ["Admin.ContentManagement.Menus.Fields.DisplayAllCategories.Hint"] = "Check to automatically display all categories in the menu. Otherwise, you may add appropriate menu items manually for all categories you want to be displayed in the menu.",
            ["Admin.ContentManagement.Menus.Fields.DisplayOrder"] = "Display order",
            ["Admin.ContentManagement.Menus.Fields.DisplayOrder.Hint"] = "The display order of the menu item. 1 represents the top of the list.",
            ["Admin.ContentManagement.Menus.Fields.LimitedToStores"] = "Limited to stores",
            ["Admin.ContentManagement.Menus.Fields.LimitedToStores.Hint"] = "Option to limit this menu to a certain store. If you have multiple stores, choose one or several from the list. If you don't use this option just leave this field empty.",
            ["Admin.ContentManagement.Menus.Fields.Name"] = "Name",
            ["Admin.ContentManagement.Menus.Fields.Name.Hint"] = "The name of the menu.",
            ["Admin.ContentManagement.Menus.Fields.Name.Required"] = "The name is required.",
            ["Admin.ContentManagement.Menus.Fields.MenuType"] = "Menu type",
            ["Admin.ContentManagement.Menus.Fields.MenuType.Hint"] = "The type of the menu.",
            ["Admin.ContentManagement.Menus.Fields.CssClass"] = "CSS class",
            ["Admin.ContentManagement.Menus.Fields.CssClass.Hint"] = "Additional CSS class will be added to the menu element. It can be useful for styling purposes.",
            ["Admin.ContentManagement.Menus.Fields.Published"] = "Published",
            ["Admin.ContentManagement.Menus.Fields.Published.Hint"] = "Check to publish this menu (visible in store). Uncheck to unpublish (menu not available in store).",
            ["Admin.ContentManagement.Menus.MenuItem.BackToMenu"] = "back to menu details",
            ["Admin.ContentManagement.Menus.MenuItem.EditDetails"] = "Edit menu item details",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Category"] = "Category",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Category.Hint"] = "Select one of the available categories.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Category.All"] = "All categories",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Manufacturer"] = "Manufacturer",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Manufacturer.Hint"] = "Select one of the available manufacturers.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Manufacturer.All"] = "All manufacturers",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.MenuItemType"] = "Menu item type",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.MenuItemType.Hint"] = "The type of the menu item.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Published"] = "Published",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Published.Hint"] = "Check to publish this menu item (visible in store). Uncheck to unpublish (menu item not available in store).",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Title"] = "Title",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Title.Hint"] = "The title of this menu item.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Title.Required"] = "The title is required.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Topic"] = "Topic",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Topic.Hint"] = "Select one of the available topics.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Topic.Required"] = "The topic is required.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Url"] = "URL",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Url.Hint"] = "Specify the URL for menu item link.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Url.Required"] = "The URL is required.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Vendor"] = "Vendor",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Vendor.Hint"] = "Select one of the available vendors.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Vendor.All"] = "All vendors",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.DisplayOrder"] = "Display order",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.DisplayOrder.Hint"] = "The display order of the menu item. 1 represents the top of the list.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.CssClass"] = "CSS class",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.CssClass.Hint"] = "Additional CSS class will be added to the menu item element. It can be useful for styling purposes.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.LimitedToStores"] = "Limited to stores",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.LimitedToStores.Hint"] = "Option to limit this menu item to a certain store. If you have multiple stores, choose one or several from the list. If you don't use this option just leave this field empty.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.MaximumNumberEntities"] = "Maximum number of elements",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.MaximumNumberEntities.Hint"] = "The maximum number of elements to display in the list or grid.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.NumberOfItemsPerGridRow"] = "Number of elements per row",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.NumberOfItemsPerGridRow.Hint"] = "The maximum number of elements that can be shown in each row of the grid.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.NumberOfSubItemsPerGridElement"] = "Number of subcategories per category item",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.NumberOfSubItemsPerGridElement.Hint"] = "Number of subcategories that can be shown for each category item in the grid.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Parent"] = "Parent menu item",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Parent.Hint"] = "Select one of the parent menu items.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Product"] = "Select a product",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Product.Hint"] = "Select one of the available products.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Product.Remove"] = "Remove",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Product.Select"] = "Select",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Product.Required"] = "The product is required",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.RouteName"] = "Standard page",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.RouteName.Hint"] = "Select one of the available pages.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.RouteName.Required"] = "The standard page is required.",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Template"] = "Template",
            ["Admin.ContentManagement.Menus.MenuItem.Fields.Template.Hint"] = "Select one of the available templates.",
            ["Admin.ContentManagement.Menus.MenuItem.Template.List.Description"] = "List - elements are displayed as a list of simple links.",
            ["Admin.ContentManagement.Menus.MenuItem.Template.Grid.Description"] = "Grid - elements are displayed in a grid layout, each genedated with an image and their associated children.",
            ["Admin.ContentManagement.Menus.MenuItem.Template.Simple.Description"] = "Simple - the selected entity is displayed as a simple link",
            ["Admin.ContentManagement.Menus.MenuItems"] = "Menu items",
            ["Admin.ContentManagement.Menus.MenuItems.SaveBeforeEdit"] = "You need to save the menu before you can add menu items for this menu page.",
            ["Admin.ContentManagement.Menus.MenuItems.AddNew"] = "Add a new menu item",
            ["Admin.ContentManagement.Menus.MenuItems.Updated"] = "The menu item has been updated successfully.",
            ["Admin.ContentManagement.Menus.MenuItems.Deleted"] = "The menu item has been deleted successfully.",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct"] = "Select product",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.Fields.ProductName"] = "Product name",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.Fields.Published"] = "Published",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.SearchKeywords"] = "Search keywords",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.SearchKeywords.Hint"] = "Search products by specific keywords.",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.SearchStore"] = "Store",
            ["Admin.ContentManagement.Menus.MenuItems.SelectProduct.SearchStore.Hint"] = "Search by a specific store.",
            ["Admin.ContentManagement.Menus.SelectRoute"] = "Select page",
            ["Admin.ContentManagement.Menus.Updated"] = "The menu has been updated successfully.",
            ["Enums.Nop.Core.Domain.Menus.MenuType.Main"] = "Main menu",
            ["Enums.Nop.Core.Domain.Menus.MenuType.Footer"] = "Footer menu",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.StandardPage"] = "Standard page",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.CustomLink"] = "Custom link",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.TopicPage"] = "Topic (page)",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.Category"] = "Categories",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.Manufacturer"] = "Manufacturer",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.Vendor"] = "Vendor",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.Product"] = "Product",
            ["Enums.Nop.Core.Domain.Menus.MenuItemType.Text"] = "Text without link",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.ApplyVendorAccount"] = "Apply for vendor account",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.CheckGiftCardBalance"] = "Check gift card balance",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Homepage"] = "Homepage",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.CompareProducts"] = "Compare products list",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.CustomerAddresses"] = "Addresses",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.CustomerInfo"] = "My account",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.CustomerOrders"] = "Orders",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.ContactUs"] = "Contact us",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Search"] = "Search",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.RecentlyViewedProducts"] = "Recently viewed products",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.NewProducts"] = "New products",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Blog"] = "Blog",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.News"] = "News archive",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Boards"] = "Forums",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.ProductTags"] = "Product tags",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Manufacturers"] = "Manufacturers",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Vendors"] = "Vendors",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Sitemap"] = "Sitemap",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Cart"] = "Shopping cart",
            ["Literals.Nop.Core.Http.NopStandardRouteNames.Wishlist"] = "Wishlist",

        }, languageId);

        #endregion
    }

    /// <summary>Collects the DOWN migration expressions</summary>
    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
