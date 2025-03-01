namespace BlazorWasmOsmOauth.Osm;

/// <summary>
///     Exceptions from the OSM client.
/// </summary>
/// <param name="message">What went wrong.</param>
public class OsmClientException(string message) : Exception(message);