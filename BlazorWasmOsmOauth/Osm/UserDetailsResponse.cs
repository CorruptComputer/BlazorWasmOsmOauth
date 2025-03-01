using System.Text.Json.Serialization;

namespace BlazorWasmOsmOauth.Osm;

/// <summary>
///   The response from the token endpoint
/// </summary>
[JsonSerializable(typeof(UserDetailsResponse))]
public sealed record UserDetailsResponse
{
    /// <summary>
    ///   The OSM API version
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; init; } = string.Empty;

    /// <summary>
    ///   The OSM API response generator
    /// </summary>
    [JsonPropertyName("generator")]
    public string? Generator { get; init; } = string.Empty;

    /// <summary>
    ///   The user details
    /// </summary>
    [JsonPropertyName("user")]
    public UserDetailsModel? User { get; init; }

    /// <summary>
    ///   The model for the user details
    /// </summary>
    [JsonSerializable(typeof(UserDetailsModel))]
    public sealed record UserDetailsModel
    {
        /// <summary>
        ///   The user's OSM ID
        /// </summary>
        [JsonPropertyName("id")]
        public long Id { get; init; }

        /// <summary>
        ///   The user's OSM display name
        /// </summary>
        [JsonPropertyName("display_name")]
        public string DisplayName { get; init; } = string.Empty;

        /// <summary>
        ///   The user's OSM account creation date
        /// </summary>
        [JsonPropertyName("account_created")]
        public DateTimeOffset AccountCreated { get; init; }

        /// <summary>
        ///   The user's OSM profile description
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; init; } = string.Empty;

        /// <summary>
        ///   The contributor terms agreement status for the user
        /// </summary>
        [JsonPropertyName("contributor_terms")]
        public UserDetailsContributorTermsModel ContributorTerms { get; init; } = new();

        /// <summary>
        ///   The images for the user
        /// </summary>
        [JsonPropertyName("img")]
        public UserDetailsImgModel UserImages { get; init; } = new();


        /// <summary>
        ///   The roles for the user, Wiki doesn't have an example for what this returns, so I have no idea.
        /// </summary>
        [JsonPropertyName("roles")]
        public List<object> Roles { get; init; } = [];

        /// <summary>
        ///   The changeset stats for the user
        /// </summary>
        [JsonPropertyName("changesets")]
        public UserDetailsChangesetsModel ChangesetStats { get; init; } = new();

        /// <summary>
        ///   Model for the user contributor terms agreements
        /// </summary>
        [JsonSerializable(typeof(UserDetailsContributorTermsModel))]
        public sealed record UserDetailsContributorTermsModel
        {
            /// <summary>
            ///   Has the user agreed to the contributor terms?
            /// </summary>
            [JsonPropertyName("agreed")]
            public bool Agreed { get; init; }

            /// <summary>
            ///   Did the user agree for their contributions to be public domain?
            /// </summary>
            [JsonPropertyName("pd")]
            public bool PublicDomain { get; init; }
        }

        /// <summary>
        ///   Model for the user details image
        /// </summary>
        [JsonSerializable(typeof(UserDetailsImgModel))]
        public sealed record UserDetailsImgModel
        {
            /// <summary>
            ///   The URL for the user's profile image
            /// </summary>
            [JsonPropertyName("href")]
            public string Href { get; init; } = string.Empty;
        }

        /// <summary>
        ///   Model for the user details changesets
        /// </summary>
        [JsonSerializable(typeof(UserDetailsChangesetsModel))]
        public sealed record UserDetailsChangesetsModel
        {
            /// <summary>
            ///   The total number of changesets for the user
            /// </summary>
            [JsonPropertyName("count")]
            public long Count { get; init; }
        }
    }
}
