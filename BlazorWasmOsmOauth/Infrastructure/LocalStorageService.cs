using System.Text.Json;
using Microsoft.JSInterop;

namespace BlazorWasmOsmOauth.Infrastructure;

/// <summary>
///   Handles all the info in localstorage
/// </summary>
/// <param name="jsRuntime"></param>
public sealed class LocalStorageService(IJSRuntime jsRuntime)
{
    /// <summary>
    ///   The localstorage key for the current users token
    /// </summary>
    public const string CurrentUserKey = "current-user-token";

    /// <summary>
    ///   The localstorage key for the osm state value, used to prevent CSRF attacks
    /// </summary>
    public const string OsmStateKey = "osm-state";

    /// <summary>
    ///   Gets the specified value from localstorage
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public async Task<T?> GetItemAsync<T>(string key, CancellationToken cancellationToken)
    {
        string? json = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", cancellationToken, key);

        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    /// <summary>
    ///   Sets the specified value in localstorage
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="cancellationToken"></param>
    /// <typeparam name="T"></typeparam>
    public async Task SetItemAsync<T>(string key, T value, CancellationToken cancellationToken)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", cancellationToken, key, JsonSerializer.Serialize(value));
    }

    /// <summary>
    ///   Removes the specified item from localstorage
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    public async Task RemoveItemAsync(string key, CancellationToken cancellationToken)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", cancellationToken, key);
    }
}