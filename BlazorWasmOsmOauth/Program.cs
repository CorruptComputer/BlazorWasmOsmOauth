using BlazorWasmOsmOauth.Infrastructure;
using BlazorWasmOsmOauth.Models;
using BlazorWasmOsmOauth.Osm;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;

namespace BlazorWasmOsmOauth;

/// <summary>
///   The entry point for the application.
/// </summary>
public static class Program
{
    /// <summary>
    ///   The entry point for the application.
    /// </summary>
    /// <param name="args">Command line args normally, no clue what this does for WASM.</param>
    /// <returns></returns>
    public static async Task Main(string[] args)
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddFluentUIComponents();

        builder.Services.AddSingleton<LocalStorageService>();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddSingleton<AppAuthenticationStateProvider>();
        builder.Services.AddSingleton<AuthenticationStateProvider>(s => s.GetRequiredService<AppAuthenticationStateProvider>());

        builder.Services.AddTransient<UnauthorizedDelegatingHandler>();

        string thisHost = new Uri(builder.HostEnvironment.BaseAddress).Authority;
        Console.WriteLine($"Host: {thisHost}");

        AppConfig? config = builder.Configuration.Get<AppConfig>();

        bool missingOsmApiBaseUrl = string.IsNullOrWhiteSpace(config?.OsmApiBaseUrl);
        bool missingClientId = string.IsNullOrWhiteSpace(config?.ClientId);
        bool missingOsmAuthBaseUrl = string.IsNullOrWhiteSpace(config?.OsmAuthBaseUrl);

        if (config == null
            || missingOsmApiBaseUrl
            || missingClientId
            || missingOsmAuthBaseUrl)
        {
            throw new AppException($"Missing {nameof(config.OsmApiBaseUrl)}: {missingOsmApiBaseUrl},\n"
                                    + $"Missing {nameof(config.ClientId)}: {missingClientId},\n"
                                    + $"Missing {nameof(config.OsmAuthBaseUrl)}: {missingOsmAuthBaseUrl}");
        }

        config.RedirectUri = $"http{(thisHost == "127.0.0.1:8123" ? string.Empty : "s")}://{thisHost}/oauth2-redirect";

        builder.Services.AddSingleton(config);

        builder.Services.AddHttpClient(OsmApiClient.HTTP_API_CLIENT_NAME, client =>
            {
                client.BaseAddress = new(config.OsmApiBaseUrl);
            })
            .AddHttpMessageHandler<UnauthorizedDelegatingHandler>();

        builder.Services.AddHttpClient(OsmApiClient.HTTP_AUTH_CLIENT_NAME, client =>
            {
                client.BaseAddress = new(config.OsmAuthBaseUrl);
            })
            .AddHttpMessageHandler<UnauthorizedDelegatingHandler>();

        builder.Services.AddTransient<OsmApiClient>();

        WebAssemblyHost app = builder.Build();

        await app.RunAsync();
    }
}
