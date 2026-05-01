using Localidades.Application.Configurations;
using Xunit;

namespace Localidades.Tests.Fixtures;

public class JwtTestFixture
{
    /// <summary>32 caracteres ASCII — tamanho mínimo razoável para assinatura HS256.</summary>
    public const string Secret = "0123456789abcdef0123456789abcdef";

    public JwtTestFixture()
    {
        Settings.SetSecret(Secret);
    }
}

[CollectionDefinition("Jwt")]
public class JwtCollection : ICollectionFixture<JwtTestFixture>
{
}
