using Nop.Core.ArtificialIntelligence;
using Nop.Services.Localization;
using Nop.Services.Logging;

namespace Nop.Services.Common.ArtificialIntelligence;

/// <summary>
/// Represent Artificial intelligence service
/// </summary>
public partial class ArtificialIntelligenceService : IArtificialIntelligenceService
{
    #region Fields

    protected readonly ArtificialIntelligenceHttpClient _httpClient;
    protected readonly ArtificialIntelligenceSettings _artificialIntelligenceSettings;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;

    #endregion

    #region Ctor

    public ArtificialIntelligenceService(ArtificialIntelligenceHttpClient httpClient,
    ArtificialIntelligenceSettings artificialIntelligenceSettings,
    ILocalizationService localizationService,
        ILogger logger)
    {
        _httpClient = httpClient;
        _artificialIntelligenceSettings = artificialIntelligenceSettings;
        _localizationService = localizationService;
        _logger = logger;
    }

    #endregion

    #region Utilities

    /// <summary>
    /// Get tone of voice instruction for query
    /// </summary>
    /// <param name="toneOfVoice">Tone of voice type</param>
    /// <param name="customToneOfVoice">Custom tone of voice instruction (applicable only for ToneOfVoiceType.Custom)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the tone of voice instruction
    /// </returns>
    protected virtual async Task<string> GetTonOfVoiceInstructionAsync(ToneOfVoiceType toneOfVoice, string customToneOfVoice)
    {
        if (toneOfVoice == ToneOfVoiceType.Custom && string.IsNullOrEmpty(customToneOfVoice))
            toneOfVoice = ToneOfVoiceType.Expert;

        return toneOfVoice switch
        {
            ToneOfVoiceType.Expert or ToneOfVoiceType.Supportive => await _localizationService.GetResourceAsync(
                $"ArtificialIntelligence.ToneOfVoice.{toneOfVoice.ToString()}"),
            ToneOfVoiceType.Custom => customToneOfVoice,
            _ => string.Empty
        };
    }

    #endregion

    #region Methods

    /// <summary>
    /// Crate product description by artificial intelligence
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="keywords">Features and keywords</param>
    /// <param name="toneOfVoice">Tone of voice</param>
    /// <param name="instruction">Special instruction</param>
    /// <param name="customToneOfVoice">Custom tone of voice (applicable only for ToneOfVoiceType.Custom)</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the generated product description
    /// </returns>
    public async Task<string> CrateProductDescriptionAsync(string productName, string keywords, ToneOfVoiceType toneOfVoice, string instruction, string customToneOfVoice = null)
    {
        return "<h1>test</h1>";
        var toneOfVoiceInstruction = await GetTonOfVoiceInstructionAsync(toneOfVoice, customToneOfVoice);

        var query = string.Format(await _localizationService.GetResourceAsync("ArtificialIntelligence.ProductDescriptionQuery"), productName, keywords, toneOfVoiceInstruction, instruction);

        try
        {
            return await _httpClient.SendQueryAsync(query);
        }
        catch (Exception e)
        {
            await _logger.ErrorAsync(e.Message, e);

            return string.Format(await _localizationService.GetResourceAsync("ArtificialIntelligence.CrateProductFailed"), e.Message);
        }
    }

    #endregion
}
