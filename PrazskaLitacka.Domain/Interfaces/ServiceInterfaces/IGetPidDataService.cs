using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Domain.PidStops.Models;
using System.Diagnostics;

namespace PrazskaLitacka.Domain.Interfaces.ServiceInterfaces;
public interface IGetPidDataService
{
    public Task<Stops> GetStationXmlAsync();
    public List<Station> GetDataForDbInserts(Stops stops);
}
