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
            {
                Console.WriteLine($"  ⚠ Provider '{providerName}' not found in config. Set GEMINI_API_KEY or OPENAI_API_KEY.");
                return;
            }

            var apiKey = providerConfig.ApiKey
                ?? Environment.GetEnvironmentVariable($"{providerName.ToUpperInvariant()}_API_KEY");

            if (string.IsNullOrEmpty(apiKey) && providerName != "lmstudio")
            {
                Console.WriteLine($"  ⚠ No API key for provider '{providerName}'. Set {providerName.ToUpperInvariant()}_API_KEY env var.");
                return;
            }

            var execution = platformConfig.Platform.Execution;
            var llm = CreateProvider(providerName, apiKey ?? "", providerConfig.Endpoint,
                execution.RetryAttempts, execution.RetryDelaySeconds, providerConfig.ContextLength);

            if (llm == null)
            {
                Console.WriteLine($"  ⚠ Unknown provider '{providerName}'. Supported: openai, gemini, lmstudio.");
                return;
            }

            var defaultModel = providerConfig.DefaultModel ?? platformConfig.Platform.DefaultModel;
            agent.LlmProcessor = async (systemPrompt, userPrompt, model, temperature, maxTokens, ct) =>
            {
                var effectiveModel = defaultModel ?? model;
                var request = new LLMRequest(systemPrompt, userPrompt,
                    effectiveModel, temperature, maxTokens);
                var response = await llm.CompleteAsync(request, ct);
                return response.Content;
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  ⚠ Failed to wire LLM provider: {ex.Message}");
        }
    }

    private static ILLMProvider? CreateProvider(string name, string apiKey, string? endpoint,
        int maxRetries, int baseDelaySeconds, int? contextLength = null)
    {
        return name.ToLowerInvariant() switch
        {
            "openai" => new OpenAIProvider(apiKey, endpoint, maxRetries, baseDelaySeconds),
            "gemini" => new GeminiProvider(apiKey, endpoint, maxRetries, baseDelaySeconds),
            "lmstudio" => new LMStudioProvider(apiKey, endpoint, maxRetries, baseDelaySeconds, contextLength ?? 4096),
            _ => null
        };
    }
}
