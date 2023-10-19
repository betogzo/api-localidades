using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using Localidades.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Localidades.Infrastructure.Repositories.Implementations;

public class StatesRepository : IStateRepository
{
    private readonly DataContext _context;

    public StatesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AlreadyExists(Estado estado) =>
        await _context.Estados.AnyAsync(
            x => x.CodigoUF == estado.CodigoUF ||
            x.SiglaUF == estado.SiglaUF ||
            x.NomeUF == estado.NomeUF
        );

    public async Task<bool> Create(Estado estado)
    {
        await _context.Estados.AddAsync(estado);
        var response = await _context.SaveChangesAsync();

        return response == 1;
    }

    public async Task<bool> Delete(Estado estadoASerDeletado)
    {
       _context.Estados.Remove(estadoASerDeletado);
        var estadoFoiDeletado = await _context.SaveChangesAsync();

        return estadoFoiDeletado == 1;
    }

    public async Task<List<Estado>> GetAllEstados() => await _context.Estados.AsNoTracking().ToListAsync();

    public async Task<Estado> GetByCodigoUF(string codigoUF) => await _context.Estados.FirstOrDefaultAsync(x => x.CodigoUF == codigoUF);

    public async Task<Estado> GetByNomeUF(string nomeUF) => await _context.Estados.FirstOrDefaultAsync(x => x.NomeUF.ToLower() == nomeUF.ToLower());

    public async Task<Estado> GetBySiglaUF(string siglaUF) => await _context.Estados.FirstOrDefaultAsync(x => x.SiglaUF.ToLower() == siglaUF.ToLower());

    public async Task<bool> Update(Estado estado)
    {
        _context.Estados.Update(estado);
        var estadoAtualizado = await _context.SaveChangesAsync();

        return estadoAtualizado == 1;
    }
}

