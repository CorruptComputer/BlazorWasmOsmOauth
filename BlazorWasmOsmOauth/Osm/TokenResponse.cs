using System.Text.Json.Serialization;

namespace BlazorWasmOsmOauth.Osm;

/// <summary>
///   The response from the token endpoint
/// </summary>
[JsonSerializable(typeof(TokenResponse))]
public sealed record TokenResponse
{
    /// <summary>
    ///   The access token
    /// </summary>
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; init; }

    /// <summary>
    ///   The token type
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? TokenType { get; init; }

    /// <summary>
    ///   The scope of the token
    /// </summary>
    [JsonPropertyName("scope")]
    public string? Scope { get; init; }

    /// <summary>
    ///   The time at which this token was created
    /// </summary>
    [JsonPropertyName("created_at")]
    public long? CreatedAt { get; init; }
}
