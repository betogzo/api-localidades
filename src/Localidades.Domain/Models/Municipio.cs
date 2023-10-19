using System.Text.Json.Serialization;

namespace Localidades.Domain.Models;

public class Municipio
{
    public int Id { get; set; }
    public string CodigoIBGE { get; set; }
    public string CodigoUF { get; set; }
    public string NomeMunicipio { get; set; }
    [JsonIgnore]
    public Estado Estado { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public DateTime ModificadoEm { get; set; } = DateTime.Now;
}