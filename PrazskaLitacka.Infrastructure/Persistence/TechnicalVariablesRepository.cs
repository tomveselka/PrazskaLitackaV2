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
public class TechnicalVariablesRepository : ITechnicalVariablesRepository
{
    private readonly ApplicationDbContext _db;
    public TechnicalVariablesRepository(ApplicationDbContext db) => _db = db;

    public Task<TechnicalVariables> GetAll() => _db.TechnicalVariables.FirstAsync();

    public async Task Update(TechnicalVariables variables)
    {
        { _db.TechnicalVariables.Update(variables); await _db.SaveChangesAsync(); }
    }
}

