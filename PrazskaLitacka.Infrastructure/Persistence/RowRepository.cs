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
public class RowRepository : IRowRepository
{
    private readonly ApplicationDbContext _db;
    public RowRepository(ApplicationDbContext db) => _db = db;
    public Task<Row?> GetById(int id) => _db.Rows.FirstOrDefaultAsync(r => r.Id == id);
}

