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
public class PointsRepository : IPointsRepository
{
    private readonly ApplicationDbContext _db;
    public PointsRepository(ApplicationDbContext db) => _db = db;
    public Task<Points?> GetByName(string name) => _db.Points.FirstOrDefaultAsync(p => p.Name == name);
    public Task<List<Points>> GetAll() => _db.Points.OrderBy(p => p.Name).ToListAsync();
    public async Task Add(Points p) { _db.Points.Add(p); await _db.SaveChangesAsync(); }
    public async Task Update(Points p) { _db.Points.Update(p); await _db.SaveChangesAsync(); }
    public async Task Delete(int id) { var x = await _db.Points.FindAsync(id); if (x != null) { _db.Points.Remove(x); await _db.SaveChangesAsync(); } }

}

