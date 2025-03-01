using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Models;
using BlazorWasmOsmOauth.Osm;

namespace BlazorWasmOsmOauth.Pages;

/// <summary>
///   OSM redirects users to this page with the supplied parameters after they have authenticated.
/// </summary>
/// <param name="navManager">The navigation manager</param>
/// <param name="config">The application configuration</param>
/// <param name="osmClient">The OSM API client</param>
/// <param name="authenticationStateProvider">The authentication state provider</param>
/// <param name="logger">The logger</param>
public partial class AuthCompletePage(NavigationManager navManager, AppConfig config, OsmApiClient osmClient, AppAuthenticationStateProvider authenticationStateProvider, ILogger<AuthCompletePage> logger) : ComponentBase
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
    public string? State { get; set; }

    /// <summary>
    ///   Event for when the page is loaded
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        logger.LogInformation("AuthCompletePage.OnInitializedAsync():\nCode: {Code}\nState: {State}", Code, State);
        await GetToken();
        await base.OnInitializedAsync();
    }

    /// <summary>
    ///   Event for when the page is changed without a full site reload
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        logger.LogInformation("AuthCompletePage.OnParametersSetAsync():\nCode: {Code}\nState: {State}", Code, State);
        await GetToken();
        await base.OnParametersSetAsync();
    }

    private async Task GetToken()
    {
        if (string.IsNullOrWhiteSpace(Code))
        {
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
