using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Models;

namespace BlazorWasmOsmOauth.Layout;

/// <summary>
///   The main layout for the application
/// </summary>
/// <param name="config"></param>
/// <param name="navManager"></param>
/// <param name="appAuthenticationStateProvider"></param>
public partial class MainLayout(AppConfig config, NavigationManager navManager, AppAuthenticationStateProvider appAuthenticationStateProvider) : LayoutComponentBase
{
    private void GoToLogin()
    {
        navManager.NavigateTo(
            navManager.GetUriWithQueryParameters($"{config.OsmAuthBaseUrl}/oauth2/authorize",
                new Dictionary<string, object?>
                {
                    { "response_type", "code" },
                    { "client_id", config.ClientId },
                    { "redirect_uri", config.RedirectUri },
                    { "scope", "read_prefs" }
                }.AsReadOnly()
            )
        );
    }

    private async Task LogOut()
    {
        await appAuthenticationStateProvider.ClearCurrentUserAsync(CancellationToken.None);
        navManager.NavigateTo("/");
    }
}
