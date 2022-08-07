namespace ShareJobsDataCli.GitHub.UploadArtifact.HttpModels.Responses;

internal sealed record GitHubListArtifactsResponse
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public List<GitHubArtifactFileContainerResponse> ArtifactFileContainers { get; init; } = new List<GitHubArtifactFileContainerResponse>();
}
