using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IRaceEntryRepository
{
    Task<RaceEntry?> GetById(int id);
    Task Add(RaceEntry entry);
    Task Update(RaceEntry entry);
    Task Delete(int id);
}
