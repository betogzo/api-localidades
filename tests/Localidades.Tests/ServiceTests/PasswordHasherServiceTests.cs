using Localidades.Application.Services;

namespace Localidades.Tests.ServiceTests;

public class PasswordHasherServiceTests
{
    private readonly PasswordHasherService _sut = new();

    [Fact]
    public void Verify_ReturnsTrue_WhenPasswordMatchesHash()
    {
        var hash = _sut.Hash("minhaSenhaSegura");

        Assert.True(_sut.Verify(hash, "minhaSenhaSegura"));
    }

    [Fact]
    public void Verify_ReturnsFalse_WhenPasswordDoesNotMatch()
    {
        var hash = _sut.Hash("senhaA");

        Assert.False(_sut.Verify(hash, "senhaB"));
    }

    [Fact]
    public void Hash_ProducesDifferentSalt_ForSamePassword()
    {
        var h1 = _sut.Hash("iguais");
        var h2 = _sut.Hash("iguais");

        Assert.NotEqual(h1, h2);
        Assert.True(_sut.Verify(h1, "iguais"));
        Assert.True(_sut.Verify(h2, "iguais"));
    }
}
