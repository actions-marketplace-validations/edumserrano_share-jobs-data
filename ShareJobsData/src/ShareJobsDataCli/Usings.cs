global using System.Diagnostics.CodeAnalysis;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using System.Net.Mime;
global using System.Runtime.CompilerServices;
global using System.Text;
global using System.Text.Json.Serialization;
global using CliFx;
global using CliFx.Attributes;
global using CliFx.Exceptions;
global using CliFx.Extensibility;
global using CliFx.Infrastructure;
global using FluentValidation;
global using FluentValidation.Results;
global using Flurl;
global using Newtonsoft.Json;
global using Newtonsoft.Json.Linq;
global using ShareJobsDataCli.CliCommands.Validators;
global using ShareJobsDataCli.GitHub;
global using ShareJobsDataCli.GitHub.Artifact.SameWorkflowRun.HttpModels.Requests;
global using ShareJobsDataCli.GitHub.Artifact.SameWorkflowRun.HttpModels.Responses;
global using ShareJobsDataCli.GitHub.Artifact.SameWorkflowRun.Types;
global using ShareJobsDataCli.GitHub.Exceptions;
global using ShareJobsDataCli.GitHub.Types;
global using ShareJobsDataCli.JobsData;
global using ShareJobsDataCli.Validations;
global using YamlDotNet.Serialization;
