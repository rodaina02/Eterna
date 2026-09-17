using Eterna.Application.Contacts;

namespace Eterna.Application.Tests;

public sealed class IpHasherTests
{
    [Fact]
    public void Hash_is_stable_for_the_same_input()
    {
        var first = IpHasher.Hash("203.0.113.10", "unit-test-secret");
        var second = IpHasher.Hash("203.0.113.10", "unit-test-secret");

        Assert.Equal(first, second);
        Assert.Equal(64, first!.Length);
        Assert.DoesNotContain("203.0.113.10", first, StringComparison.Ordinal);
    }

    [Fact]
    public void Hash_changes_when_the_secret_changes()
    {
        var first = IpHasher.Hash("203.0.113.10", "secret-a");
        var second = IpHasher.Hash("203.0.113.10", "secret-b");

        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Hash_changes_when_the_ip_changes()
    {
        var first = IpHasher.Hash("203.0.113.10", "unit-test-secret");
        var second = IpHasher.Hash("198.51.100.20", "unit-test-secret");

        Assert.NotEqual(first, second);
        Assert.Equal(64, second!.Length);
    }

    [Fact]
    public void Hash_is_null_when_secret_is_missing()
    {
        Assert.Null(IpHasher.Hash("203.0.113.10", ""));
        Assert.Null(IpHasher.Hash(null, "secret"));
    }
}
