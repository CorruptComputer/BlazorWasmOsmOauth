namespace BlazorWasmOsmOauth.AppHost;

/// <summary>
///   Entry point for the application.
/// </summary>
public static class Program
{
    /// <summary>
    ///   The main character of the project.
    /// </summary>
    /// <param name="args">Arg, I'm a pirate.</param>
    public static async Task Main(string[] args)
    {
        await DistributedApplication.CreateBuilder(args).BuildAppHost().RunAppHostAsync();
    }

    private static DistributedApplication BuildAppHost(this IDistributedApplicationBuilder builder)
    {
        const string discoveryName = "blazorwasmosmoauth-web";
        builder.AddProject<Projects.BlazorWasmOsmOauth>(discoveryName)
               .WithExternalHttpEndpoints();

        return builder.Build();
    }

    private static async Task RunAppHostAsync(this DistributedApplication app)
    {
        await app.RunAsync();
    }
}