namespace BlazorWasmOsmOauth;

/// <summary>
///     Exceptions from this application should throw this.
/// </summary>
/// <param name="message">What went wrong.</param>
public class AppException(string message) : Exception(message);