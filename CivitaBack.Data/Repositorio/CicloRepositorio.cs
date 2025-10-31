using CivitaBack.Domain.Entities;
using CivitaBack.Data.EF;
using CivitaBack.Domain.Interfaces.Repositorios;
using Microsoft.EntityFrameworkCore;

namespace CivitaBack.Data.Repositorio
{

    public class CicloRepositorio : ICicloRepositorio
    {
        private readonly AppDbContext _context;
        public CicloRepositorio(AppDbContext context)
        {
            _context = context;
        }

    }
}
