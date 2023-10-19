using System.Text.Json.Serialization;

namespace Localidades.Domain.Models;

public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Email { get; set; }
    [JsonIgnore]
    public string Senha { get; set; }
    public DateTime RegistradoEm { get; set; }
    [JsonIgnore]
    public string Role { get; set; } = "admin"; // apenas para fins de testes de desenvolvimento
}