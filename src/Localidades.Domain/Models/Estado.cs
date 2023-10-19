using System.Text.Json.Serialization;

namespace Localidades.Domain.Models;

public class Estado
{
    public int Id { get; set; }
    public string CodigoUF { get; set; }
    public string SiglaUF { get; set; }
    public string NomeUF { get; set; }
    [JsonIgnore]
    public ICollection<Municipio>? Municipios { get; set; }
    public DateTime CriadoEm = DateTime.Now;
    public DateTime ModificadoEm = DateTime.Now;
}