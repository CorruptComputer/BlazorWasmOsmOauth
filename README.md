# BlazorWasmOsmOauth

Example setup for Oauth with Blazor Webassembly.

On the OSM side you'll want to configure the 'Redirect URI' to:
- `http(s)://<domain or 127.0.0.1>/oauth2-redirect`

The OSM Client ID and Client Secret are set via one of the following: 
- `BlazorWasmOsmOauth/wwwroot/appsettings.Development.json`
- `BlazorWasmOsmOauth/wwwroot/appsettings.Production.json`

With the one for Development being used when running this locally and the one for Production being used when accessed via a deployed build. The OSM instance can also be configured from here, though for this example they are both pointing to the dev instance.