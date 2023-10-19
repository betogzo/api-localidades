using Localidades.Domain.Models;

namespace Localidades.Domain.Interfaces.Repositories;

public interface ICityRepository
{
    Task<bool> Create(Municipio municipio);
    Task<bool> Update(Municipio municipio);
    Task<bool> Delete(Municipio municipio);
    Task<bool> AlreadyExists(Municipio municipio);
    Task<Municipio> GetByCodigoIBGE(string codigoIBGE);
    Task<List<Municipio>> GetByName(string nomeMunicipio);
    Task<List<Municipio>> GetByNameAndState(string nomeMunicipio, string siglaUF);
    Task<List<Municipio>> GetAllCitiesBySiglaUF(string siglaUF, int skip, int take);
    Task<int> CountCitites();
    Task<int> CountCititesByState(string siglaUF);
    Task<List<Municipio>> GetAllCities(int skip = -1, int take = -1);
    void BulkInsertion(List<Municipio> municipios);
}