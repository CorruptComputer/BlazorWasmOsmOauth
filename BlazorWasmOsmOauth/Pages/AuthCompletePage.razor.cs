using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Models;
using BlazorWasmOsmOauth.Osm;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorWasmOsmOauth.Pages;

/// <summary>
///   OSM redirects users to this page with the supplied parameters after they have authenticated.
/// </summary>
/// <param name="navManager">The navigation manager</param>
/// <param name="config">The application configuration</param>
/// <param name="osmClient">The OSM API client</param>
/// <param name="authenticationStateProvider">The authentication state provider</param>
/// <param name="localStorageService"></param>
/// <param name="dialogService"></param>
public partial class AuthCompletePage(NavigationManager navManager, AppConfig config, OsmApiClient osmClient,
    AppAuthenticationStateProvider authenticationStateProvider, LocalStorageService localStorageService, IDialogService dialogService)
    : ComponentBase
{
    /// <summary>
    ///   The code supplied by OSM.
    /// </summary>
    [Parameter]
    [SupplyParameterFromQuery(Name = "code")]
    public string? Code { get; set; }

    /// <summary>
    ///   Hopefully the same state token that was provided to OSM when the user was redirected to the OSM login page, used to prevent CSRF attacks.
    ///   If this is not the same, or is missing, then we should not trust the code supplied by OSM.
    ///   
    ///   This can be any string, though for sake of simplicity this example uses a GUID.
    /// </summary>
    [Parameter]
    [SupplyParameterFromQuery(Name = "state")]
    public Guid? State { get; set; }

    /// <summary>
    ///   Event for when the page is loaded
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await GetToken();
        await base.OnInitializedAsync();
    }

    private async Task GetToken()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
            navManager.NavigateTo("/");
            return;
        }

        if (!await ValidateState())
        {
            navManager.NavigateTo("/");
            return;
        }

        string? pkce = await localStorageService.GetItemAsync<string>(LocalStorageService.OsmPkceKey, CancellationToken.None);

        if (string.IsNullOrWhiteSpace(pkce))
        {
            // Wait for the dialog to close, then redirect to the home page
            IDialogReference dialogReference = await dialogService.ShowErrorAsync("The PKCE value is missing.", title: "Missing PKCE");
            await dialogReference.Result;

            navManager.NavigateTo("/");
            return;
        }

        TokenResponse? tokenResp = await osmClient.GetTokenAsync(Code, config.RedirectUri, config.ClientId, pkce);

        if (tokenResp == null)
        {
            // Wait for the dialog to close, then redirect to the home page
            IDialogReference dialogReference = await dialogService.ShowErrorAsync("The OSM login failed.", title: "Login failed");
            await dialogReference.Result;

            navManager.NavigateTo("/");
            return;
        }

        await authenticationStateProvider.SetCurrentUserAsync(tokenResp, CancellationToken.None);

        navManager.NavigateTo("/");
    }

    private async Task<bool> ValidateState() 
    {
        Guid? stateFromStorage = await localStorageService.GetItemAsync<Guid>(LocalStorageService.OsmStateKey, CancellationToken.None);
        if (State == null
            || stateFromStorage == null
            || State != stateFromStorage)
        {
            // Wait for the dialog to close, then return
            IDialogReference dialogReference = await dialogService.ShowErrorAsync("The state value supplied by OSM is invalid.", title: "Invalid State");
            await dialogReference.Result;

            
            return false;
        }

        return true;
    }
}
