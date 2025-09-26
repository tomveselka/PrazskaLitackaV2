using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Domain.Entities;

namespace PrazskaLitacka.Domain.Interfaces;
public interface ITechnicalVariablesRepository
{
    Task<TechnicalVariables> GetAll();
    Task Update(TechnicalVariables variables);
}
