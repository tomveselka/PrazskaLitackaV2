using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Infrastructure.Persistence;
public class RaceRepository
{
    private readonly ApplicationDbContext _db;
    public RaceRepository(ApplicationDbContext db) => _db = db;

    public Task<Race?> GetById(int id) =>
        _db.Races
            .Include(r => r.RaceEntries).ThenInclude(e => e.Rows)
            .Include(r => r.BonusStations)
            .Include(r => r.BonusLines)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task Add(Race race)
    {
        _db.Races.Add(race);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Race race)
    {
        _db.Races.Update(race);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _db.Races.FindAsync(id);
        if (entity != null)
        {
            _db.Races.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}

