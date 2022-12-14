using static ShareJobsDataCli.Features.SetData.Outputs.ParseSetDataCommandOutputResult;

namespace ShareJobsDataCli.Features.SetData.Outputs;

internal abstract class SetDataCommandOutput
{
    private SetDataCommandOutput()
    {
    }

    public static ParseSetDataCommandOutputResult FromOption(IConsole console, string outputOptionValue)
    {
        return outputOptionValue switch
        {
            var _ when string.Equals(outputOptionValue, "none", StringComparison.OrdinalIgnoreCase) => new NoOutput(),
            var _ when string.Equals(outputOptionValue, "strict-json", StringComparison.OrdinalIgnoreCase) => new StrictJsonOutput(console),
            var _ when string.Equals(outputOptionValue, "github-step-json", StringComparison.OrdinalIgnoreCase) => new GitHubStepJsonOutput(console),
            _ => new UnknownOutput(outputOptionValue),
        };
    }

    public abstract Task WriteToConsoleAsync(JobData jobData);

    public sealed class NoOutput : SetDataCommandOutput
    {
        public override Task WriteToConsoleAsync(JobData jobData) => Task.CompletedTask;
    }

    public sealed class StrictJsonOutput : SetDataCommandOutput
    {
        private readonly GitHubActionStrictJsonStepOutput _strictJsonOutput;

        public StrictJsonOutput(IConsole console)
        {
            _strictJsonOutput = new GitHubActionStrictJsonStepOutput(console);
        }

        public override Task WriteToConsoleAsync(JobData jobData)
        {
            var json = jobData.AsJson();
            return _strictJsonOutput.WriteToConsoleAsync(json);
        }
    }

    public sealed class GitHubStepJsonOutput : SetDataCommandOutput
    {
        private readonly GitHubActionStepJsonStepOutput _stepJsonOutput;

        public GitHubStepJsonOutput(IConsole console)
        {
            _stepJsonOutput = new GitHubActionStepJsonStepOutput(console);
        }

        public override Task WriteToConsoleAsync(JobData jobData)
        {
            var keysAndValues = jobData.AsKeyValues();
            return _stepJsonOutput.WriteToConsoleAsync(keysAndValues);
        }
    }
}
