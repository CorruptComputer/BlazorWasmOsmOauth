using System.Security.Cryptography;
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
    private string SourceRevisionId => config.SourceRevisionId;

    private async Task GoToLogin()
    {
        // Note: This is a simple example, in a real application you'd probably want 
        //       to use a more secure method of generating these values.

        Guid state = Guid.NewGuid();
        await localStorageService.SetItemAsync(LocalStorageService.OsmStateKey, state, CancellationToken.None);

        Guid pkce1 = Guid.NewGuid();
        Guid pkce2 = Guid.NewGuid();
        string pkce = pkce1.ToString("N") + pkce2.ToString("N");
        await localStorageService.SetItemAsync(LocalStorageService.OsmPkceKey, pkce, CancellationToken.None);

        byte[] pkceSha256 = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(pkce));

        navManager.NavigateTo(
            navManager.GetUriWithQueryParameters($"{config.OsmAuthBaseUrl}/oauth2/authorize",
                new Dictionary<string, object?>
                {
                    { "response_type", "code" },
                    { "client_id", config.ClientId },
                    { "redirect_uri", config.RedirectUri },
                    { "scope", "read_prefs" },
                    { "state", state },
                    { "code_challenge", Convert.ToBase64String(pkceSha256).Replace('+', '-').Replace('/', '_').TrimEnd('=') },
                    { "code_challenge_method", "S256" }
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
