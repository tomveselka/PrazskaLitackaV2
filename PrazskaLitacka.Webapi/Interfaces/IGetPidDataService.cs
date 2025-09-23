using PidStops.Models;
using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.Webapi.XmlModels;

namespace PrazskaLitacka.Webapi.Interfaces;
public interface IGetPidDataService
{
    Task<Stops> GetStationXmlAsync();
    public PidDataDto GetDataForDbInserts(Stops stops);
}
