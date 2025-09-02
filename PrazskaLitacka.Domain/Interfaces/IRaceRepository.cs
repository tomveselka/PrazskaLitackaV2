using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IRaceRepository
{
    Task<Race?> GetById(int id);
    Task Add(Race race);
    Task Update(Race race);
    Task Delete(int id);
}
