using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Settings;

/// <summary>
/// Represents a artificial intelligence settings model
/// </summary>
public partial record ArtificialIntelligenceSettingsModel : BaseNopModel
{
    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.Enable")]
    public bool Enabled { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.ProviderType")]
    public int ProviderTypeId { get; set; }
    public IList<SelectListItem> AvailableProviderType { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.GeminiApiKey")]
    public string GeminiApiKey { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.ChatGptApiKey")]
    public string ChatGptApiKey { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.DeepSeekApiKey")]
    public string DeepSeekApiKey { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Settings.Catalog.ArtificialIntelligence.ProductDescriptionQuery")]
    public string ProductDescriptionQuery { get; set; }

    #endregion
}
