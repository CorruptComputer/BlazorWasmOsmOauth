namespace BlazorWasmOsmOauth;

/// <summary>
///   The main view for the application
/// </summary>
/// <param name="navManager"></param>
public partial class App(NavigationManager navManager)
{
    /// <summary>
    ///   Redirect from localhost to 127.0.0.1, as OSM only allows the latter.
    ///   
    ///   Sadly this is a limitation of both OSM and Aspire,
    ///   as Aspire replaces 127.0.0.1 with localhost in the URL when running locally.
    /// </summary>
    protected override void OnInitialized()
    {
        if (navManager.Uri.Contains("localhost"))
        {
            navManager.NavigateTo("http://127.0.0.1:8123");
        }
    }
}
