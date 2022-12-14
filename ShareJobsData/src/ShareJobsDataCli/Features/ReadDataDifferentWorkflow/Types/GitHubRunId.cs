namespace ShareJobsDataCli.Features.ReadDataDifferentWorkflow.Types;

internal sealed class GitHubRunId
{
    private readonly string _value;

    public GitHubRunId(string runId)
    {
        _value = runId.NotNullOrWhiteSpace();
    }

    public static implicit operator string(GitHubRunId runId)
    {
        return runId._value;
    }

    public override string ToString() => this;
}
