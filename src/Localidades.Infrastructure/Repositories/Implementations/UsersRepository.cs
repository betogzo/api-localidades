using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Localidades.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Localidades.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly DataContext _context;

    public UsersRepository(DataContext context)
    {
        _context = context;
    }


    public async Task<bool> AlreadyExistsByEmail(string email) => await _context.Usuarios.AnyAsync(x => x.Email == email);

    public async Task<bool> Create(Usuario novoUsuario)
    {
        await _context.Usuarios.AddAsync(novoUsuario);
        var response = await _context.SaveChangesAsync();

        return response == 1;
    }

    public async Task<Usuario> GetByEmail(string email) => await _context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
}

