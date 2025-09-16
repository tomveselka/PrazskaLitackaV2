using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prazska_litacka.webapi;
using System.Xml.Serialization;
using PrazskaLitacka.Webapi.Interfaces;
using PrazskaLitacka.Infrastructure.Exceptions;
using PrazskaLitacka.Webapi.XmlModels;

namespace PrazskaLitacka.Infrastructure.Services.Impl;
public class GetStationXml : IGetStationXml
{
    private readonly HttpClient _httpClient;

    public GetStationXml(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlDataClient");
    }

    public async Task<Stops> GetStationXmlAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        var serializer = new XmlSerializer(typeof(Stops));
        var stops = (Stops?)serializer.Deserialize(stream);

        if (stops == null)
        {
            throw new StopsNotRetrievedException();
        }
        return stops;
    }
}

