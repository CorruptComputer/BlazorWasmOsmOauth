using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace BlazorWasmOsmOauth.Osm;

/// <summary>
///   The client for the OSM API.
/// </summary>
/// <param name="httpClientFactory"></param>
public class OsmApiClient(IHttpClientFactory httpClientFactory)
{
    /// <summary>
    ///   The name of the api client as registered in the App startup.
    /// </summary>
    public const string HttpApiClientName = "OsmApiClient";

    /// <summary>
    ///   The name of the auth client as registered in the App startup.
    /// </summary>
    public const string HttpAuthClientName = "OsmAuthClient";

    private readonly HttpClient _apiClient = httpClientFactory.CreateClient(HttpApiClientName);

    private readonly HttpClient _authClient = httpClientFactory.CreateClient(HttpAuthClientName);

    /// <summary>
    ///   Get the user token from the OSM API, using the code from the OAuth2 flow. Or null if the request fails.
    /// </summary>
    /// <returns></returns>
    public async Task<TokenResponse?> GetTokenAsync(string code, string redirectUri, string clientId, string pkce)
    {
        string queryStr = "grant_type=authorization_code"
                            + $"&code={HttpUtility.UrlEncode(code)}"
                            + $"&redirect_uri={HttpUtility.UrlEncode(redirectUri)}"
                            + $"&client_id={HttpUtility.UrlEncode(clientId)}"
                            + $"&code_verifier={HttpUtility.UrlEncode(pkce)}";

        HttpResponseMessage response = await _authClient.PostAsync($"/oauth2/token?{queryStr}", new StringContent(string.Empty, Encoding.UTF8, "application/x-www-form-urlencoded"));

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }

    /// <summary>
    ///   Gets the logged in user's details from the OSM API. Or null if the request fails.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<UserDetailsResponse?> GetUserDetailsAsync(TokenResponse token)
    {
        _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        HttpResponseMessage response = await _apiClient.GetAsync("/api/0.6/user/details.json");

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserDetailsResponse>();
    }
}