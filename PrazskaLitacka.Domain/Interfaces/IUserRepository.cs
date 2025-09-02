using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IUserRepository
{
    Task<User?> GetByLogin(string login);
    Task Add(User user);
    Task Update(User user);
}
