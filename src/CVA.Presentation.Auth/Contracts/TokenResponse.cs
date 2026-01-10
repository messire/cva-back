using System.Text.Json.Serialization;

namespace CVA.Presentation.Auth;

internal sealed class TokenResponse
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; init; } = string.Empty;

    [JsonPropertyName("error")]
    public string? Error { get; init; }

    [JsonPropertyName("error_description")]
    public string? ErrorDescription { get; init; }
}