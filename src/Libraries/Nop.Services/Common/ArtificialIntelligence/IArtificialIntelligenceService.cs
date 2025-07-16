using Nop.Core.ArtificialIntelligence;

namespace Nop.Services.Common.ArtificialIntelligence;

/// <summary>
/// Artificial intelligence service interface
/// </summary>
public partial interface IArtificialIntelligenceService
{
    /// <summary>
    /// Crate product description by artificial intelligence
    /// </summary>
    /// <param name="productName">Product name</param>
    /// <param name="keywords">Features and keywords</param>
    /// <param name="toneOfVoice">Tone of voice</param>
    /// <param name="instruction">Special instruction</param>
    /// <param name="customToneOfVoice">Custom tone of voice (applicable only for ToneOfVoiceType.Custom)</param>
    /// <param name="languageId">The language identifier</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the generated product description
    /// </returns>
    Task<string> CrateProductDescriptionAsync(string productName, string keywords, ToneOfVoiceType toneOfVoice, string instruction, string customToneOfVoice = null, int languageId=0);
}