using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.Webapi.XmlModels;

namespace PrazskaLitacka.Webapi.Interfaces;
public interface IGetStationXml
{
    Task<Stops> GetStationXmlAsync();
}
