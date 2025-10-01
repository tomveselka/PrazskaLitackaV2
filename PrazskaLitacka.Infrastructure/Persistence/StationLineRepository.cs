using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.Interfaces;

namespace PrazskaLitacka.Infrastructure.Persistence;
public class StationLineRepository : IStationLineRepository
{
    private readonly ApplicationDbContext _db;
    public StationLineRepository(ApplicationDbContext db) => _db = db;
    public Task<StationLine?> GetById(int id) => _db.StationLines.FirstOrDefaultAsync(s => s.Id == id);
    public Task<List<StationLine>> GetByBeginningOfName(string name) => _db.StationLines.Where(s => s.Name.StartsWith(name)).ToListAsync();
}

