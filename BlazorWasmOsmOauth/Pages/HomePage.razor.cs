using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Osm;

namespace BlazorWasmOsmOauth.Pages;

/// <summary>
///   
/// </summary>
/// <param name="osmClient"></param>
/// <param name="authenticationStateProvider"></param>
public partial class HomePage(OsmApiClient osmClient, AppAuthenticationStateProvider authenticationStateProvider) : ComponentBase
{
    private string LoggedInUserDisplayName { get; set; } = string.Empty;
    private string LoggedInUserChangesetCount { get; set; } = string.Empty;

    /// <summary>
    ///   Event for when the page is loaded
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await GetLoggedInUserProfile();
        await base.OnInitializedAsync();
    }

    /// <summary>
    ///   Event for when the page is changed without a full site reload
    /// </summary>
    /// <returns></returns>
    protected override async Task OnParametersSetAsync()
    {
        await GetLoggedInUserProfile();
        await base.OnParametersSetAsync();
    }

    private async Task GetLoggedInUserProfile()
    {
        TokenResponse? token = await authenticationStateProvider.GetCurrentUserAsync(CancellationToken.None);
        if (string.IsNullOrWhiteSpace(token?.AccessToken))
        {
            return;
        }

        UserDetailsResponse? userDetails = await osmClient.GetUserDetailsAsync(token);
        Console.WriteLine(userDetails);

        if (userDetails != null)
        {
            LoggedInUserDisplayName = userDetails.User?.DisplayName ?? string.Empty;
            LoggedInUserChangesetCount = $"{userDetails.User?.ChangesetStats?.Count}";
        }
    }
}
