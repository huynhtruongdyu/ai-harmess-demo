using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AiCompany.Integrations.GitHub;

public class GitHubClient
{
    private readonly HttpClient _httpClient;
    private readonly string _owner;
    private readonly string _repo;

    public GitHubClient(string token, string owner, string repo)
    {
        _owner = owner;
        _repo = repo;
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://api.github.com")
        };
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AiCompany", "1.0"));
    }

    public async Task<GitHubPR?> CreatePullRequestAsync(
        string title, string body, string head, string @base,
        CancellationToken ct = default)
    {
        var payload = new { title, body, head, @base };
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(
            $"/repos/{_owner}/{_repo}/pulls", content, ct);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<GitHubPR>(responseJson);
    }

    public async Task<IReadOnlyList<GitHubPR>> ListPullRequestsAsync(
        string? state = null, CancellationToken ct = default)
    {
        var query = state != null ? $"?state={state}" : "";
        var response = await _httpClient.GetAsync(
            $"/repos/{_owner}/{_repo}/pulls{query}", ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<List<GitHubPR>>(json) ?? new List<GitHubPR>();
    }

    public void Dispose() => _httpClient.Dispose();
}

public class GitHubPR
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonPropertyName("html_url")]
    public string? HtmlUrl { get; set; }

    [JsonPropertyName("body")]
    public string? Body { get; set; }

    [JsonPropertyName("user")]
    public GitHubUser? User { get; set; }

    [JsonPropertyName("head")]
    public GitHubBranch? Head { get; set; }

    [JsonPropertyName("base")]
    public GitHubBranch? Base { get; set; }
}

public class GitHubUser
{
    [JsonPropertyName("login")]
    public string? Login { get; set; }
}

public class GitHubBranch
{
    [JsonPropertyName("ref")]
    public string? Ref { get; set; }

    [JsonPropertyName("sha")]
    public string? Sha { get; set; }
}
