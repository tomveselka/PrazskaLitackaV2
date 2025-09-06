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
public class BonusLineRepository : IBonusLineRepository
{
    private readonly ApplicationDbContext _db;
    public BonusLineRepository(ApplicationDbContext db) => _db = db;
    public Task<BonusLine?> GetById(int id) => _db.BonusLines.FirstOrDefaultAsync(r => r.Id == id);
    public Task<List<BonusLine>> GetAllForRace(int raceId) => _db.BonusLines.Select(x => x).Where(x => x.RaceId == raceId).ToListAsync();
    public async Task Add(BonusLine e) { _db.BonusLines.Add(e); await _db.SaveChangesAsync(); }
    public async Task Update(BonusLine e) { _db.BonusLines.Update(e); await _db.SaveChangesAsync(); }
    public async Task Delete(int id) { var x = await _db.BonusLines.FindAsync(id); if (x != null) { _db.BonusLines.Remove(x); await _db.SaveChangesAsync(); } }

}

