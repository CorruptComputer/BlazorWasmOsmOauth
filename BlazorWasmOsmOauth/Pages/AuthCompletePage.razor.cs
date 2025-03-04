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
    ///   The state supplied by OSM, used to prevent CSRF attacks.
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

        if (State == null
            || State != await localStorageService.GetItemAsync<Guid>(LocalStorageService.OsmStateKey, CancellationToken.None))
        {
            // Wait for the dialog to close, then redirect to the home page
            IDialogReference dialogReference = await dialogService.ShowErrorAsync("The state value supplied by OSM is invalid.", title: "Invalid State");
            await dialogReference.Result;

            navManager.NavigateTo("/");
            return;
        }

        TokenResponse? tokenResp = await osmClient.GetTokenAsync(Code, config.RedirectUri, config.ClientId, config.ClientSecret);

        Console.WriteLine(tokenResp);

        if (tokenResp != null)
        {
            await authenticationStateProvider.SetCurrentUserAsync(tokenResp, CancellationToken.None);
        }

        navManager.NavigateTo("/");
    }
}
