using NLWebNet.Models;
using NLWebNet.Services;
using System.Runtime.CompilerServices;

namespace NLWebNet.Tests.Services;

/// <summary>
/// Test implementation of IResultGenerator for unit testing.
/// </summary>
public class TestResultGenerator : IResultGenerator
{
    public IEnumerable<NLWebResult>? Results { get; set; }
    public string? Summary { get; set; }
    public string? GeneratedResponse { get; set; }
    public List<string>? StreamingChunks { get; set; }
    public bool ShouldThrowCancellation { get; set; }

    public Task<IEnumerable<NLWebResult>> GenerateListAsync(string query, string? site = null, CancellationToken cancellationToken = default)
    {
        if (ShouldThrowCancellation)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        return Task.FromResult(Results ?? Enumerable.Empty<NLWebResult>());
    }

    public Task<(string Summary, IEnumerable<NLWebResult> Results)> GenerateSummaryAsync(string query, IEnumerable<NLWebResult> results, CancellationToken cancellationToken = default)
    {
        if (ShouldThrowCancellation)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        var resultsList = Results ?? results.ToList();
        var summary = Summary ?? $"Summary for '{query}' with {resultsList.Count()} results";
        return Task.FromResult((summary, resultsList));
    }

    public Task<(string GeneratedResponse, IEnumerable<NLWebResult> Results)> GenerateResponseAsync(string query, IEnumerable<NLWebResult> results, CancellationToken cancellationToken = default)
    {
        if (ShouldThrowCancellation)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        var resultsList = Results ?? results.ToList();
        var response = GeneratedResponse ?? $"Generated response for '{query}' based on {resultsList.Count()} results";
        return Task.FromResult((response, resultsList));
    }

    public async IAsyncEnumerable<string> GenerateStreamingResponseAsync(string query, IEnumerable<NLWebResult> results, QueryMode mode, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (ShouldThrowCancellation)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        var chunks = StreamingChunks ?? new List<string> { "Streaming", " response", " for", $" '{query}'" };
        
        foreach (var chunk in chunks)
        {
            yield return chunk;
            // Remove artificial delay for faster test execution
            await Task.Yield(); // Just yield control without delay
        }
    }

    public void SetResults(IEnumerable<NLWebResult> results)
    {
        Results = results.ToList();
    }
}
