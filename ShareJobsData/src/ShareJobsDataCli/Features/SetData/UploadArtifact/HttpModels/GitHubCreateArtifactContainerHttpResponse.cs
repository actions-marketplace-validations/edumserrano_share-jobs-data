namespace ShareJobsDataCli.Features.SetData.UploadArtifact.HttpModels;

#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.
internal sealed record GitHubCreateArtifactContainerHttpResponse(
    long ContainerId,
    long Size,
    string FileContainerResourceUrl,
    string Type,
    string Name,
    string Url,
    DateTime ExpiresOn);
#pragma warning disable CA1812 // Avoid uninstantiated internal classes. Referenced via JSON generic type deserialization.

internal sealed class GitHubCreateArtifactContainerHttpResponseValidator : AbstractValidator<GitHubCreateArtifactContainerHttpResponse>
{
    public GitHubCreateArtifactContainerHttpResponseValidator()
    {
        RuleFor(x => x.FileContainerResourceUrl)
            .Must(fileContainerResourceUrl => Uri.TryCreate(fileContainerResourceUrl, default(UriCreationOptions), out var _))
            .WithMessage(x => $"$.fileContainerResourceUrl is not a valid URL. Actual value: '{x.FileContainerResourceUrl}'.");
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(_ => "$.name must have a value.");
    }
}
