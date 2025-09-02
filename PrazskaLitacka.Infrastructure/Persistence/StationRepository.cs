using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Infrastructure.Persistence;
public class StationRepository
{
    private readonly ApplicationDbContext _db;
    public StationRepository(ApplicationDbContext db) => _db = db;
    public Task<Station?> GetById(int id) => _db.Stations.Include(s => s.Lines).FirstOrDefaultAsync(s => s.Id == id);
    public Task<List<Station>> GetByBeginningOfName(string name) => _db.Stations.Where(s => s.Name.StartsWith(name)).ToListAsync();
    public Task<List<Station>> GetAll() => _db.Stations.Include(s => s.Lines).ToListAsync();

}

