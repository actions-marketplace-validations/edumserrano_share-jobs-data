using ShareJobsDataCli.GitHub.UploadArtifact.HttpModels;

namespace ShareJobsDataCli.GitHub.UploadArtifact;

internal class GitHubUploadArtifactHttpClient
{
    private readonly HttpClient _httpClient;

    public GitHubUploadArtifactHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient.NotNull();
    }

    public static HttpClient CreateHttpClient(string actionRuntimeToken, string repository)
    {
        actionRuntimeToken.NotNullOrWhiteSpace();
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", actionRuntimeToken);
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", $"application/json;api-version={GitHubApiVersion.Latest}");
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", $"share-job-data-cli:{repository}");
        return httpClient;
    }

    public async Task UploadArtifactAsync(
        GitHubUploadArtifactContainerUrl containerUrl,
        GitHubUploadArtifact artifact)
    {
        var createArtifactFileContainerResponse = await CreateArtifactFileContainerAsync(containerUrl, artifact.ContainerName);
        Console.WriteLine($"createArtifactFileContainerResponse: {createArtifactFileContainerResponse}");

        var uploadFileResponse = await UploadArtifactFileAsync(createArtifactFileContainerResponse.FileContainerResourceUrl, artifact);
        Console.WriteLine($"uploadFileResponse: {uploadFileResponse}");

        var finalizeArtifactResponse = await FinalizeArtifactContainerAsync(containerUrl, artifact, uploadFileResponse.FileLength);
        Console.WriteLine($"finalizeArtifactResponse: {finalizeArtifactResponse}");
    }

    private async Task<UpdateArtifactFileContainerResponse> CreateArtifactFileContainerAsync(GitHubUploadArtifactContainerUrl containerUrl, string containerName)
    {
        using var createArtifactFileContainerHttpRequest = new HttpRequestMessage(HttpMethod.Post, containerUrl);
        var containerRequest = new CreateArtifactFileContainerRequest(containerName);
        createArtifactFileContainerHttpRequest.Content = JsonContent.Create(containerRequest);
        var createArtifactFileContainerHttpResponse = await _httpClient.SendAsync(createArtifactFileContainerHttpRequest);
        createArtifactFileContainerHttpResponse.EnsureSuccessStatusCode(); // TODO improve, check status code and throw error message with body if fails, add extension method for this EnsureSucessStatusCodeWithError()
        var createArtifactFileContainerResponse = await createArtifactFileContainerHttpResponse.Content.ReadFromJsonAsync<UpdateArtifactFileContainerResponse>();
        return createArtifactFileContainerResponse.NotNull(); // TODO throw same type of exception that EnsureSucessStatusCodeWithError instead of using NotNull workaround
    }

    private async Task<UploadArtifactFileResponse> UploadArtifactFileAsync(
        string fileContainerResourceUrl,
        GitHubUploadArtifact artifact)
    {
        var contentBytes = Encoding.UTF8.GetBytes(artifact.FilePayload);
        using var stream = new MemoryStream(contentBytes);
        var uploadFileUrl = fileContainerResourceUrl.SetQueryParam("itemPath", artifact.FilePath);
        using var uploadFileHttpRequest = new HttpRequestMessage(HttpMethod.Put, uploadFileUrl);
        uploadFileHttpRequest.Content = new StreamContent(stream);
        uploadFileHttpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Octet);
        uploadFileHttpRequest.Content.Headers.ContentRange = new ContentRangeHeaderValue(from: 0, to: contentBytes.Length - 1, length: contentBytes.Length);
        var uploadFileHttpResponse = await _httpClient.SendAsync(uploadFileHttpRequest);
        uploadFileHttpResponse.EnsureSuccessStatusCode(); // TODO improve, check status code and throw error message with body if fails, add extension method for this EnsureSucessStatusCodeWithError()
        var updateArtifactResponse = await uploadFileHttpResponse.Content.ReadFromJsonAsync<UploadArtifactFileResponse>();
        return updateArtifactResponse.NotNull(); // TODO throw same type of exception that EnsureSucessStatusCodeWithError instead of using NotNull workaround
    }

    private async Task<UpdateArtifactFileContainerResponse> FinalizeArtifactContainerAsync(
        GitHubUploadArtifactContainerUrl containerUrl,
        GitHubUploadArtifact artifact,
        int containerSize)
    {
        var finalizeArtifactContainerRequest = new FinalizeArtifactContainerRequest(containerSize);
        var setArtifactSizeUrl = $"{containerUrl}".SetQueryParam("artifactName", artifact.ContainerName);
        using var finalizeArtifactContainerHttpRequest = new HttpRequestMessage(HttpMethod.Patch, setArtifactSizeUrl);
        finalizeArtifactContainerHttpRequest.Content = JsonContent.Create(finalizeArtifactContainerRequest);
        var finalizeArtifactContainerHttpResponse = await _httpClient.SendAsync(finalizeArtifactContainerHttpRequest);
        finalizeArtifactContainerHttpResponse.EnsureSuccessStatusCode(); // TODO improve, check status code and throw error message with body if fails, add extension method for this EnsureSucessStatusCodeWithError()
        var finalizeArtifactContainerResponse = await finalizeArtifactContainerHttpResponse.Content.ReadFromJsonAsync<UpdateArtifactFileContainerResponse>();
        return finalizeArtifactContainerResponse.NotNull(); // TODO throw same type of exception that EnsureSucessStatusCodeWithError instead of using NotNull workaround
    }
}
