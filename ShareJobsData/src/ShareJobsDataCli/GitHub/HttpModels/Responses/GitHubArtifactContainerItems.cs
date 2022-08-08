namespace ShareJobsDataCli.GitHub.HttpModels.Responses;

public record GitHubArtifactContainerItems
{
    public int Count { get; init; }

    [JsonPropertyName("value")]
    public IReadOnlyList<GitHubArtifactContainerItem> ContainerItems { get; init; } = new List<GitHubArtifactContainerItem>();
}
