using static ShareJobsDataCli.GitHub.Artifacts.CurrentWorkflowRun.DownloadArtifactFile.Results.DownloadArtifactFileFromCurrentWorkflowResult;

namespace ShareJobsDataCli.CliCommands.Commands.ReadDataCurrentWorkflow;

internal static class CommandExceptionExtensions
{
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static CommandException ToCommandException(this Error error)
    {
        error.NotNull();
        var details = error switch
        {
            ArtifactNotFound artifactNotFound => $"Couldn't find artifact '{artifactNotFound.ArtifactContainerName}'.",
            ArtifactContainerItemNotFound artifactContainerItemNotFound => $"Couldn't find artifact file '{artifactContainerItemNotFound.ArtifactItemFilePath}'.",
            FailedToListWorkflowRunArtifacts failedToListWorkflowRunArtifacts => failedToListWorkflowRunArtifacts.JsonHttpError.ToErrorDetails("listing GitHub workflow artifacts"),
            FailedToGetContainerItems failedToGetContainerItems => failedToGetContainerItems.JsonHttpError.ToErrorDetails("retrieving GitHub workflow artifact container items"),
            FailedToDownloadArtifact failedToDownloadArtifact => failedToDownloadArtifact.FailedStatusCodeHttpResponse.ToErrorDetails("downloading GitHub artifact"),
            _ => throw UnexpectedTypeException.Create(error),
        };
        var exceptionMessage = new ReadDataFromCurrentWorkflowCommandExceptionMessage(details);
        return exceptionMessage.ToCommandException();
    }
}
