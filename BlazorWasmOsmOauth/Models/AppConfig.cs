using System.Text.Json.Serialization;

namespace BlazorWasmOsmOauth.Models;

/// <summary>
///   Configuration for the application.
/// </summary>
public sealed class AppConfig
{
    /// <summary>
    ///   The base URL for the OSM API
    /// </summary>
    public string OsmApiBaseUrl { get; set; } = string.Empty;

    /// <summary>
    ///   Client ID from OSM to use, get from: https://www.openstreetmap.org/oauth2/applications
    /// </summary>
    public string ClientId { get; set; } = string.Empty;

    /// <summary>
    ///   Client Secret from OSM to use, get from: https://www.openstreetmap.org/oauth2/applications
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    ///   Base URL for the OSM OAuth API
    /// </summary>
    public string OsmAuthBaseUrl { get; set; } = string.Empty;

    /// <summary>
    ///   The redirect URI for the OSM OAuth API, not in appsettings we build it when registering this config.
    /// </summary>
    [JsonIgnore]
    public string RedirectUri { get; set; } = string.Empty;
}