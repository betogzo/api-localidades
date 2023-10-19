using Localidades.Domain.Models;

namespace Localidades.Domain.Interfaces.Repositories;

public interface IStateRepository
{
    Task<bool> Create(Estado estado);
    Task<bool> Update(Estado estado);
    Task<bool> Delete(Estado estado);
    Task<bool> AlreadyExists(Estado estado);
    Task<Estado> GetByCodigoUF(string codigoUF);
    Task<Estado> GetBySiglaUF(string siglaUF);
    Task<Estado> GetByNomeUF(string nomeUF);
    Task<List<Estado>> GetAllEstados();
}

