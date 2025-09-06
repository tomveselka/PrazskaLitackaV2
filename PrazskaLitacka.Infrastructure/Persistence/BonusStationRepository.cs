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
public class BonusStationRepository : IBonusStationRepository
{
    private readonly ApplicationDbContext _db;
    public BonusStationRepository(ApplicationDbContext db) => _db = db;
    public Task<BonusStation?> GetById(int id) => _db.BonusStations.FirstOrDefaultAsync(r => r.Id == id);
    public Task<List<BonusStation>> GetAllForRace(int raceId) => _db.BonusStations.Select(x=>x).Where(x =>x.RaceId==raceId).ToListAsync();
    public async Task Add(BonusStation e) { _db.BonusStations.Add(e); await _db.SaveChangesAsync(); }
    public async Task Update(BonusStation e) { _db.BonusStations.Update(e); await _db.SaveChangesAsync(); }
    public async Task Delete(int id) { var x = await _db.BonusStations.FindAsync(id); if (x != null) { _db.BonusStations.Remove(x); await _db.SaveChangesAsync(); } }

}

