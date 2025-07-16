using Microsoft.AspNetCore.Mvc;
using Nop.Core.ArtificialIntelligence;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Components;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Components;

/// <summary>
/// Represents view component to use artificial intelligence service to generate product's full description
/// </summary>
public class FullDescriptionGeneratorViewComponent : NopViewComponent
{
    #region Filds

    protected readonly ArtificialIntelligenceSettings _artificialIntelligenceSettings;

    #endregion

    #region Ctor

    public FullDescriptionGeneratorViewComponent(ArtificialIntelligenceSettings artificialIntelligenceSettings)
    {
        _artificialIntelligenceSettings = artificialIntelligenceSettings;
    }

    #endregion

    #region Methods

    public IViewComponentResult Invoke(object additionalData)
    {
        if (additionalData is not FullDescriptionGeneratorModel model || !_artificialIntelligenceSettings.Enabled)
            return Content(string.Empty);

        return View(model);
    }

    #endregion

    #region Nested class

    public partial record FullDescriptionGeneratorModel : BaseNopModel
    {
        public virtual string ProductNameElementId { get; set; }
        public virtual int LanguageId { get; set; }
    }

    #endregion
}
