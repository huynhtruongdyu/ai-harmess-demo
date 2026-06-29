using AiCompany.Memory;
using FluentAssertions;
using Xunit;

namespace AiCompany.Tests.Unit.Core;

public class MemoryStoreTests
{
    [Fact]
    public async Task ReadWrite_Roundtrip_PersistsContent()
    {
        var store = new FileSystemMemoryStore();
        var testContent = "# Test\nHello, world!";

        await store.WriteAsync("_test_store", testContent);
        var result = await store.ReadAsync("_test_store");

        result.Should().Be(testContent);

        // Cleanup
        var path = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "memory", "_test_store.md");
        if (File.Exists(path)) File.Delete(path);
    }

    [Fact]
    public async Task Read_NonExistentStore_ReturnsEmpty()
    {
        var store = new FileSystemMemoryStore();
        var result = await store.ReadAsync("_nonexistent_" + Guid.NewGuid());

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Exists_AfterWrite_ReturnsTrue()
    {
        var store = new FileSystemMemoryStore();
        await store.WriteAsync("_test_exists", "content");

        var exists = await store.ExistsAsync("_test_exists");
        exists.Should().BeTrue();

        var path = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "memory", "_test_exists.md");
        if (File.Exists(path)) File.Delete(path);
    }

    [Fact]
    public async Task Append_AddsToExistingContent()
    {
        var store = new FileSystemMemoryStore();
        await store.WriteAsync("_test_append", "Line 1");
        await store.AppendAsync("_test_append", "Line 2");

        var result = await store.ReadAsync("_test_append");
        result.Should().Contain("Line 1");
        result.Should().Contain("Line 2");

        var path = Path.Combine(Directory.GetCurrentDirectory(), ".ai", "memory", "_test_append.md");
        if (File.Exists(path)) File.Delete(path);
    }
}
