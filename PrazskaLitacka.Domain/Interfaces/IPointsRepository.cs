using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IPointsRepository
{
    Task<Points?> GetByName(string name);
    Task<List<Points>> GetAll();
    Task Add(Points p);
    Task Update(Points p);
    Task Delete(int id);
}
