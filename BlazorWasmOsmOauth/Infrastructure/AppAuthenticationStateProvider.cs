using System.Security.Claims;
using BlazorWasmOsmOauth.Osm;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorWasmOsmOauth.Infrastructure;

/// <summary>
///   Provides the state of authentication
/// </summary>
/// <param name="localStorageService"></param>
public class AppAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
{
    /// <inheritdoc />
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        TokenResponse? currentUser = await GetCurrentUserAsync(CancellationToken.None);

        if (currentUser == null)
        {
            return new(new());
        }

        return new(new(new ClaimsIdentity([], authenticationType: nameof(AppAuthenticationStateProvider))));
    }

    /// <summary>
    ///   Sets the current user
    /// </summary>
    /// <param name="currentUser"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SetCurrentUserAsync(TokenResponse currentUser, CancellationToken cancellationToken)
    {
        await localStorageService.SetItemAsync(LocalStorageService.CurrentUserKey, currentUser, cancellationToken);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    /// <summary>
    ///   Clears the current user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task ClearCurrentUserAsync(CancellationToken cancellationToken)
    {
        await localStorageService.RemoveItemAsync(LocalStorageService.CurrentUserKey, cancellationToken);

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    /// <summary>
    ///   Gets the current user
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<TokenResponse?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        return localStorageService.GetItemAsync<TokenResponse>(LocalStorageService.CurrentUserKey, cancellationToken);
    }
}