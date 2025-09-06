using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IBonusStationRepository
{
    Task<BonusStation?> GetById(int id);
    Task<List<BonusStation>> GetAllForRace(int raceId);
    Task Add(BonusStation e);
    Task Update(BonusStation e);
    Task Delete(int id);
}
