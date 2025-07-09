using FluentMigrator;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Menus;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Vendors;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Common;
using Nop.Services.Configuration;

namespace Nop.Web.Framework.Migrations.UpgradeTo490;

[NopUpdateMigration("2025-02-26 00:00:00", "4.90", UpdateMigrationType.Settings)]
public class SettingMigration : MigrationBase
{
    /// <summary>Collect the UP migration expressions</summary>
    public override void Up()
    {
        if (!DataSettingsManager.IsDatabaseInstalled())
            return;

        //do not use DI, because it produces exception on the installation process
        var settingService = EngineContext.Current.Resolve<ISettingService>();

        //#6590
        var adminAreaSettings = settingService.LoadSetting<AdminAreaSettings>();
        if (!settingService.SettingExists(adminAreaSettings, settings => settings.UseStickyHeaderLayout))
        {
            adminAreaSettings.UseStickyHeaderLayout = false;
            settingService.SaveSetting(adminAreaSettings, settings => settings.UseStickyHeaderLayout);
        }

        //#7387
        var productEditorSettings = settingService.LoadSetting<ProductEditorSettings>();
        if (!settingService.SettingExists(productEditorSettings, settings => settings.AgeVerification))
        {
            productEditorSettings.AgeVerification = false;
            settingService.SaveSetting(productEditorSettings, settings => settings.AgeVerification);
        }

        //#2184
        var vendorSettings = settingService.LoadSetting<VendorSettings>();
        if (!settingService.SettingExists(vendorSettings, settings => settings.MaximumProductPicturesNumber))
        {
            vendorSettings.MaximumProductPicturesNumber = 5;
            settingService.SaveSetting(vendorSettings, settings => settings.MaximumProductPicturesNumber);
        }

        //#7571
        var captchaSettings = settingService.LoadSetting<CaptchaSettings>();
        if (!settingService.SettingExists(captchaSettings, settings => settings.ShowOnCheckGiftCardBalance))
        {
            captchaSettings.ShowOnCheckGiftCardBalance = true;
            settingService.SaveSetting(captchaSettings, settings => settings.ShowOnCheckGiftCardBalance);
        }

        //#5818
        var mediaSettings = settingService.LoadSetting<MediaSettings>();
        if (!settingService.SettingExists(mediaSettings, settings => settings.AutoOrientImage))
        {
            mediaSettings.AutoOrientImage = false;
            settingService.SaveSetting(mediaSettings, settings => settings.AutoOrientImage);
        }

        //#1892
        if (!settingService.SettingExists(adminAreaSettings, settings => settings.MinimumDropdownItemsForSearch))
        {
            adminAreaSettings.MinimumDropdownItemsForSearch = 50;
            settingService.SaveSetting(adminAreaSettings, settings => settings.MinimumDropdownItemsForSearch);
        }

        //#7405
        var catalogSettings = settingService.LoadSetting<CatalogSettings>();
        if (!settingService.SettingExists(catalogSettings, settings => settings.ExportImportCategoryUseLimitedToStores))
        {
            catalogSettings.ExportImportCategoryUseLimitedToStores = false;
            settingService.SaveSetting(catalogSettings, settings => settings.ExportImportCategoryUseLimitedToStores);
        }

        //#7477
        var pdfSettings = settingService.LoadSetting<PdfSettings>();
        var pdfSettingsFontFamily = settingService.GetSetting("pdfsettings.fontfamily");
        if (pdfSettingsFontFamily is not null)
            settingService.DeleteSetting(pdfSettingsFontFamily);

        if (!settingService.SettingExists(pdfSettings, settings => settings.RtlFontName))
        {
            pdfSettings.RtlFontName = NopCommonDefaults.PdfRtlFontName;
            settingService.SaveSetting(pdfSettings, settings => pdfSettings.RtlFontName);
        }

        if (!settingService.SettingExists(pdfSettings, settings => settings.LtrFontName))
        {
            pdfSettings.LtrFontName = NopCommonDefaults.PdfLtrFontName;
            settingService.SaveSetting(pdfSettings, settings => pdfSettings.LtrFontName);
        }

        if (!settingService.SettingExists(pdfSettings, settings => settings.BaseFontSize))
        {
            pdfSettings.BaseFontSize = 10f;
            settingService.SaveSetting(pdfSettings, settings => pdfSettings.BaseFontSize);
        }

        if (!settingService.SettingExists(pdfSettings, settings => settings.ImageTargetSize))
        {
            pdfSettings.ImageTargetSize = 200;
            settingService.SaveSetting(pdfSettings, settings => pdfSettings.ImageTargetSize);
        }

        //#820
        var currencySettings = settingService.LoadSetting<CurrencySettings>();
        if (!settingService.SettingExists(currencySettings, settings => settings.DisplayCurrencySymbolInCurrencySelector))
        {
            currencySettings.DisplayCurrencySymbolInCurrencySelector = false;
            settingService.SaveSetting(currencySettings, settings => settings.DisplayCurrencySymbolInCurrencySelector);
        }

        //#1779
        var customerSettings = settingService.LoadSetting<CustomerSettings>();
        if (!settingService.SettingExists(customerSettings, settings => settings.NotifyFailedLoginAttempt))
        {
            customerSettings.NotifyFailedLoginAttempt = false;
            settingService.SaveSetting(customerSettings, settings => settings.NotifyFailedLoginAttempt);
        }

        //#7630
        var taxSettings = settingService.LoadSetting<TaxSettings>();

        if (!settingService.SettingExists(taxSettings, settings => settings.HmrcApiUrl))
        {
            taxSettings.HmrcApiUrl = "https://api.service.hmrc.gov.uk";
            settingService.SaveSetting(taxSettings, settings => taxSettings.HmrcApiUrl);
        }

        if (!settingService.SettingExists(taxSettings, settings => settings.HmrcClientId))
        {
            taxSettings.HmrcClientId = string.Empty;
            settingService.SaveSetting(taxSettings, settings => taxSettings.HmrcClientId);
        }

        if (!settingService.SettingExists(taxSettings, settings => settings.HmrcClientSecret))
        {
            taxSettings.HmrcClientSecret = string.Empty;
            settingService.SaveSetting(taxSettings, settings => taxSettings.HmrcClientSecret);
        }

        //#1266
        var orderSettings = settingService.LoadSetting<OrderSettings>();
        if (!settingService.SettingExists(orderSettings, settings => settings.CustomerOrdersPageSize))
        {
            orderSettings.CustomerOrdersPageSize = 10;
            settingService.SaveSetting(orderSettings, settings => settings.CustomerOrdersPageSize);
        }

        //#7390
        var menuSettings = settingService.LoadSetting<MenuSettings>();
        if (!settingService.SettingExists(menuSettings, settings => settings.MaximumNumberEntities))
        {
            menuSettings.MaximumNumberEntities = 8;
            settingService.SaveSetting(menuSettings, settings => settings.MaximumNumberEntities);
        }

        if (!settingService.SettingExists(menuSettings, settings => settings.NumberOfItemsPerGridRow))
        {
            menuSettings.NumberOfItemsPerGridRow = 4;
            settingService.SaveSetting(menuSettings, settings => settings.NumberOfItemsPerGridRow);
        }

        if (!settingService.SettingExists(menuSettings, settings => settings.NumberOfSubItemsPerGridElement))
        {
            menuSettings.NumberOfSubItemsPerGridElement = 3;
            settingService.SaveSetting(menuSettings, settings => settings.NumberOfSubItemsPerGridElement);
        }

        if (!settingService.SettingExists(menuSettings, settings => settings.MaximumMainMenuLevels))
        {
            menuSettings.MaximumMainMenuLevels = 2;
            settingService.SaveSetting(menuSettings, settings => settings.MaximumMainMenuLevels);
        }

        if (!settingService.SettingExists(menuSettings, settings => settings.GridThumbPictureSize))
        {
            menuSettings.GridThumbPictureSize = 340;
            settingService.SaveSetting(menuSettings, settings => settings.GridThumbPictureSize);
        }

        var displayDefaultMenuItemSettings = settingService.GetAllSettings()
            .Where(s => s.Name.StartsWith("DisplayDefaultMenuItemSettings.", StringComparison.OrdinalIgnoreCase));

        if (displayDefaultMenuItemSettings.Any())
            settingService.DeleteSettingsAsync(displayDefaultMenuItemSettings.ToList());

        var useajaxloadmenu = settingService.GetSetting("catalogsettings.useajaxloadmenu");
        if (useajaxloadmenu is not null)
            settingService.DeleteSettingAsync(useajaxloadmenu);
    }

    public override void Down()
    {
        //add the downgrade logic if necessary 
    }
}
