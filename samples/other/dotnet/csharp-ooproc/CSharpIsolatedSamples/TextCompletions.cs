// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenAI;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CSharpIsolatedSamples;

/// <summary>
/// These samples show how to use the OpenAI Completions APIs. For more details on the Completions APIs, see
/// https://platform.openai.com/docs/guides/completion.
/// </summary>
public static class TextCompletions
{
    /// <summary>
    /// This sample demonstrates the "templating" pattern, where the function takes a parameter
    /// and embeds it into a text prompt, which is then sent to the OpenAI completions API.
    /// </summary>
    [Function(nameof(WhoIs))]
    public static string WhoIs(
        [HttpTrigger(AuthorizationLevel.Function, Route = "whois/{name}")] HttpRequestData req,
        [TextCompletionInput("Who is {name}?")] TextCompletionResponse response)
    {
        return response.Content;
    }

    /// <summary>
    /// This sample takes a prompt as input, sends it directly to the OpenAI completions API, and results the 
    /// response as the output.
    /// </summary>
    [Function(nameof(GenericCompletion))]
    public static IActionResult GenericCompletion(
        [HttpTrigger(AuthorizationLevel.Function, "post")] PromptPayload payload,
        [TextCompletionInput("{Prompt}", Model = "gpt-35-turbo")] TextCompletionResponse response,
        ILogger log)
    {
        // ToDo: Investigate payload null reference exception
        // log.LogInformation("Prompt = {prompt}, Response = {response}", payload?.Prompt, response);
        string text = response.Content;
        return new OkObjectResult(text);
    }

    public record PromptPayload(string Prompt);
}