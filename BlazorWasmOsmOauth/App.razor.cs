namespace BlazorWasmOsmOauth;

/// <summary>
///   The main view for the application
/// </summary>
/// <param name="navManager"></param>
public partial class App(NavigationManager navManager)
{
    /// <summary>
    ///   Redirect from localhost to 127.0.0.1, as OSM only allows the latter
    /// </summary>
    protected override void OnInitialized()
    {
        if (navManager.Uri.Contains("localhost"))
        {
            navManager.NavigateTo("http://127.0.0.1:8123");
        }
    }
}
