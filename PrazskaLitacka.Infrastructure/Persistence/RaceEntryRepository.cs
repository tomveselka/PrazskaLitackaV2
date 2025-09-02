using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrazskaLitacka.Domain.DbContexts;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Infrastructure.Persistence;
public class RaceEntryRepository
{
    private readonly ApplicationDbContext _db;
    public RaceEntryRepository(ApplicationDbContext db) => _db = db;

    public Task<RaceEntry?> GetById(int id) =>
        _db.RaceEntries
            .Include(e => e.Rows)
            .Include(e => e.Race).ThenInclude(r => r.BonusLines)
            .Include(e => e.Race).ThenInclude(r => r.BonusStations)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task Add(RaceEntry entry)
    {
        _db.RaceEntries.Add(entry);
        await _db.SaveChangesAsync();
    }

    public async Task Update(RaceEntry entry)
    {
        _db.RaceEntries.Update(entry);
        await _db.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _db.RaceEntries.FindAsync(id);
        if (entity != null)
        {
            _db.RaceEntries.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}

