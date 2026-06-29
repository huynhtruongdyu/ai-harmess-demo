using AiCompany.Agents;
using AiCompany.Configuration;
using AiCompany.Integrations.LLM;

namespace AiCompany.Cli;

public static class LLMHelper
{
    public static void TryWireLlmProcessor(AgentRuntime agent, ConfigLoader configLoader)
    {
        try
        {
            var platformConfig = configLoader.LoadPlatformConfig();
            var providerName = platformConfig.Platform.DefaultProvider;
            if (!platformConfig.Providers.TryGetValue(providerName, out var providerConfig))
                return;

            var apiKey = providerConfig.ApiKey
                ?? Environment.GetEnvironmentVariable($"{providerName.ToUpperInvariant()}_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
                return;

            var execution = platformConfig.Platform.Execution;
            var llm = CreateProvider(providerName, apiKey, providerConfig.Endpoint,
                execution.RetryAttempts, execution.RetryDelaySeconds);

            if (llm == null) return;

            agent.LlmProcessor = async (systemPrompt, userPrompt, model, temperature, maxTokens, ct) =>
            {
                var request = new LLMRequest(systemPrompt, userPrompt,
                    model, temperature, maxTokens);
                var response = await llm.CompleteAsync(request, ct);
                return response.Content;
            };
        }
        catch
        {
            // LLM wiring is optional; fall back to standalone mode
        }
    }

    private static ILLMProvider? CreateProvider(string name, string apiKey, string? endpoint,
        int maxRetries, int baseDelaySeconds)
    {
        return name.ToLowerInvariant() switch
        {
            "openai" => new OpenAIProvider(apiKey, endpoint, maxRetries, baseDelaySeconds),
            "gemini" => new GeminiProvider(apiKey, endpoint, maxRetries, baseDelaySeconds),
            _ => null
        };
    }
}
