using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface IStationRepository
{
    Task<Station?> GetById(int id);
    Task<List<Station>> GetByBeginningOfName(string name);
    Task<List<Station>> GetAll();
}
