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
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;
    public UserRepository(ApplicationDbContext db) => _db = db;
    public Task<User?> GetByLogin(string login) => _db.Users.FirstOrDefaultAsync(u => u.Login == login);
    public async Task Add(User user) { _db.Users.Add(user); await _db.SaveChangesAsync(); }
    public async Task Update(User user) { _db.Users.Update(user); await _db.SaveChangesAsync(); }

}

