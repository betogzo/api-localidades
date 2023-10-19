using Localidades.Domain.Interfaces.Repositories;
using Localidades.Domain.Models;
using EFCore.BulkExtensions;
using Localidades.Infrastructure.Data; 
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Localidades.Infrastructure.Repositories.Implementations
{
    public class CitiesRepository : ICityRepository
    {
        private readonly DataContext _context;

        public CitiesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AlreadyExists(Municipio municipio) => await _context.Municipios.AsNoTracking().AnyAsync(x => 
            x.CodigoIBGE == municipio.CodigoIBGE || 
            x.NomeMunicipio == municipio.NomeMunicipio);

        public async void BulkInsertion(List<Municipio> municipios)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                await _context.BulkInsertAsync(municipios);
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> Create(Municipio municipio)
        {
            _context.Municipios.Add(municipio);
            var municipioCriado = await _context.SaveChangesAsync();

            return municipioCriado == 1;
        }

        public async Task<bool> Delete(Municipio municipioASerDeletado)
        {
            _context.Municipios.Remove(municipioASerDeletado);
            var municipioFoiDeletado = await _context.SaveChangesAsync();

            return municipioFoiDeletado == 1;
        }

        public async Task<List<Municipio>> GetAllCities(int skip = -1, int take = -1)
        {
            var query = _context.Municipios.AsNoTracking();

            if (skip >= 0)
                query = query.Skip(skip);

            if (take > 0)
                query = query.Take(take);

            return await query.Include(x => x.Estado).ToListAsync();
        }

        public async Task<int> CountCitites()
        {
            return await _context.Municipios.AsNoTracking().CountAsync();
        }

        public async Task<int> CountCititesByState(string siglaUF)
        {
            return await _context.Municipios.AsNoTracking().Where(x => x.Estado.SiglaUF == siglaUF).CountAsync();
        }

        public async Task<List<Municipio>> GetAllCitiesBySiglaUF(string siglaUF, int skip, int take) =>
            await _context.Municipios.AsNoTracking().Include(x => x.Estado).Where(x => x.Estado.SiglaUF == siglaUF).Skip(skip).Take(take).ToListAsync();

        public async Task<Municipio> GetByCodigoIBGE(string codigoIBGE) => 
            await _context.Municipios.Include(x => x.Estado).FirstOrDefaultAsync(x => x.CodigoIBGE == codigoIBGE);

        public async Task<List<Municipio>> GetByName(string nomeMunicipio) => 
            await _context.Municipios.AsNoTracking().Include(x => x.Estado).Where(m => EF.Functions.Collate(m.NomeMunicipio, "Latin1_General_CI_AI") == nomeMunicipio).ToListAsync();

        public async Task<bool> Update(Municipio municipioASerAtualizado)
        {
            _context.Municipios.Update(municipioASerAtualizado);
            var municipioAtualizado = await _context.SaveChangesAsync();

            return municipioAtualizado == 1;
        }

        public async Task<List<Municipio>> GetByNameAndState(string nomeMunicipio, string siglaUF)
        {
            return await _context.Municipios.Include(x => x.Estado).Where(x => x.NomeMunicipio == nomeMunicipio && x.Estado.SiglaUF == siglaUF).ToListAsync();
        }
    }
}
