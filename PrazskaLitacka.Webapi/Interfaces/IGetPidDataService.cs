using PidStops.Models;
using PrazskaLitacka.Domain.Entities;
using PrazskaLitacka.Webapi.Interfaces;

namespace PrazskaLitacka.Webapi.Interfaces;
public interface IGetPidDataService
{
    public Task<Stops> GetStationXmlAsync();
    public List<Station> GetDataForDbInserts(Stops stops);
    public Task UpdateTables(List<Station> stations);
}
