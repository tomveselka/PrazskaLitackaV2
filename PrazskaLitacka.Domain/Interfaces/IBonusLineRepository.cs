using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IBonusLineRepository
{
    Task<BonusLine?> GetById(int id);
    Task Add(BonusLine e);
    Task Update(BonusLine e);
    Task Delete(int id);
}
