# BlazorWasmOsmOauth

Example setup for Oauth with Blazor Webassembly.

When creating the Application in OSM make sure to uncheck the 'Confidential application?" box.

Also configure the 'Redirect URI' to:
- `http(s)://<domain or 127.0.0.1>/oauth2-redirect`

Since this is an entirely client-side app there can be no client secret, so when OSM tells it to you just ignore that and copy the client ID.

The OSM Client ID is set via one of the following: 
- `BlazorWasmOsmOauth/wwwroot/appsettings.Development.json`
- `BlazorWasmOsmOauth/wwwroot/appsettings.Production.json`

With the one for Development being used when running this locally and the one for Production being used when accessed via a deployed build. The OSM instance can also be configured from here, though for this example they are both pointing to the dev instance.