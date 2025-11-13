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
public class StationRepository : IStationRepository
{
    private readonly ApplicationDbContext _db;
    public StationRepository(ApplicationDbContext db) => _db = db;
    public Task<Station?> GetById(int id) => _db.Stations.Include(s => s.Lines).FirstOrDefaultAsync(s => s.Id == id);
    public Task<List<Station>> GetByBeginningOfName(string name, int page, int recordsPerPage) => _db.Stations
        .OrderBy(s=>s.Name)
        .Where(s => s.Name.StartsWith(name))
        .Skip((page - 1) * recordsPerPage)
        .Take(recordsPerPage)
        .ToListAsync();
    public Task<List<Station>> GetAll() => _db.Stations.Include(s => s.Lines).ToListAsync();

    public async Task DropAllUploadNew(IEnumerable<Station> stationList)
    {
        using var transaction = await _db.Database.BeginTransactionAsync();

        try
        {
            //TODO - move this to repositories so it is easier to mock
            // Clear dependent table first (StationLines), then Stations
            await _db.Stations.ExecuteDeleteAsync();
            await _db.StationLines.ExecuteDeleteAsync();

            await _db.SaveChangesAsync();

            // Insert new data
            await _db.Stations.AddRangeAsync(stationList);
            await _db.SaveChangesAsync();

            var technicalVariables = await _db.TechnicalVariables.FirstAsync();
            technicalVariables.TimeOfLastStationUpdate = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

