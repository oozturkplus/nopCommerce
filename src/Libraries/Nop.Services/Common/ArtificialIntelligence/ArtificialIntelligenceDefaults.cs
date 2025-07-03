namespace Nop.Services.Common.ArtificialIntelligence;

/// <summary>
/// Represents artificial intelligence constants
/// </summary>
public partial class ArtificialIntelligenceDefaults
{
    #region ChatGPT

    /// <summary>
    /// Gets ChatGPT API model
    /// </summary>
    public static string ChatGptApiModel => "gpt-4.1";

    /// <summary>
    /// Gets base ChatGPT API URL
    /// </summary>
    public static string ChatGptBaseApiUrl => "https://api.openai.com/v1/responses";

    #endregion

    #region Gemini

    /// <summary>
    /// Gets base Gemini API URL
    /// </summary>
    public static string GeminiBaseApiUrl => "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";

    /// <summary>
    /// Gets a header of the API key authorization
    /// </summary>
    public static string GeminiApiKeyHeader => "x-goog-api-key";

    #endregion

    #region DeepSeek

    /// <summary>
    /// Gets DeepSeek API model
    /// </summary>
    public static string DeepSeekApiModel => "deepseek-chat";

    /// <summary>
    /// Gets base DeepSeek API URL
    /// </summary>
    public static string DeepSeekBaseApiUrl => "https://api.deepseek.com/chat/completions";

    #endregion

    /// <summary>
    /// Gets a period (in seconds) before the request times out
    /// </summary>
    public static int RequestTimeout => 30;
}