using System.IdentityModel.Tokens.Jwt;
using Localidades.Application.Endpoints;
using Localidades.Domain.Models;
using Localidades.Tests.Fixtures;

namespace Localidades.Tests.ServiceTests;

[Collection("Jwt")]
public class TokenServiceTests
{
    [Fact]
    public void GenerateToken_ReturnsParsableJwt_WithEmailAndRoleClaims()
    {
        var sut = new TokenService();
        var usuario = new Usuario { Email = "user@test.com", Role = "admin" };

        var token = sut.GenerateToken(usuario);

        Assert.False(string.IsNullOrWhiteSpace(token));
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        // O handler pode serializar tipos de claim como URI ou nome curto (p. ex. unique_name).
        Assert.Contains(jwt.Claims, c => c.Value == usuario.Email);
        Assert.Contains(jwt.Claims, c => c.Value == usuario.Role);
    }
}
