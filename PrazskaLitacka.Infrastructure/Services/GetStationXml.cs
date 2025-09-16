using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrazskaLitacka.Webapi.Interfaces;

namespace PrazskaLitacka.Infrastructure.Services;
public class GetStationXml : IGetStationXml
{
    private readonly HttpClient _httpClient;

    public GetStationXml(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient("XmlDataClient");
    }

    public async Task<string> GetStationXmlAsync()
    {
        var response = await _httpClient.GetAsync(_httpClient.BaseAddress);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
    }
}

