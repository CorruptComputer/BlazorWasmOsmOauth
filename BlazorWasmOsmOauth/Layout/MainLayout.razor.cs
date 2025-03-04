using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Models;

namespace BlazorWasmOsmOauth.Layout;

/// <summary>
///   The main layout for the application
/// </summary>
/// <param name="config"></param>
/// <param name="navManager"></param>
/// <param name="appAuthenticationStateProvider"></param>
/// <param name="localStorageService"></param>
public partial class MainLayout(AppConfig config, NavigationManager navManager, AppAuthenticationStateProvider appAuthenticationStateProvider,
    LocalStorageService localStorageService) : LayoutComponentBase
{
    private async Task GoToLogin()
    {
        Guid state = Guid.NewGuid();
        await localStorageService.SetItemAsync(LocalStorageService.OsmStateKey, state, CancellationToken.None);

        navManager.NavigateTo(
            navManager.GetUriWithQueryParameters($"{config.OsmAuthBaseUrl}/oauth2/authorize",
                new Dictionary<string, object?>
                {
                    { "response_type", "code" },
                    { "client_id", config.ClientId },
                    { "redirect_uri", config.RedirectUri },
                    { "scope", "read_prefs" },
                    { "state", state }
                }.AsReadOnly()
            )
        );
    }

    private async Task LogOut()
    {
        await appAuthenticationStateProvider.ClearCurrentUserAsync(CancellationToken.None);
        await localStorageService.RemoveItemAsync(LocalStorageService.OsmStateKey, CancellationToken.None);
        navManager.NavigateTo("/");
    }
}
