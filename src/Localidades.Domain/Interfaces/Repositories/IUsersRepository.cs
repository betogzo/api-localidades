using Localidades.Domain.Models;

namespace Localidades.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<bool> AlreadyExistsByEmail(string email);
    Task<Usuario> GetByEmail(string email);
    Task<bool> Create(Usuario novoUsuario);
}

