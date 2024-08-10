#nullable enable

using CityPopulationWebService.Extexsion;
using CityPopulationWebService.JsonConverter;
using CityPopulationWebService.Model;
using CityPopulationWebService.Service.Interface;
using System.IO.Compression;
using System.Text.Json;
namespace CityPopulationWebService.Service;

public sealed class CityService : ICityService
{

    private readonly static string SourceDataUrl = "http://bulk.openweathermap.org/sample/current.city.list.json.gz";

    private static IReadOnlyList<City> Cities = Array.Empty<City>();

    public CityService()
    {
        LoadCityData();
    }


    public IReadOnlyList<City> Search(string? name = "")
    {
        var cities = Cities;

        var searchTerms = new HashSet<string>() { };
        if (!string.IsNullOrEmpty(name))
        {
            searchTerms = name.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        }
        var ids = searchTerms.Where(sc => double.TryParse(sc, out var id)).Select(sc => double.Parse(sc)).ToHashSet();

        if (ids.Count > 0)
        {
            cities = cities.Where(c => ids.Contains(c.Id)).ToList();
        }
        else
        {
            foreach (var term in searchTerms)
            {
                cities = cities.Where(c => c.Name.ContainsIgnoreCase(term)).ToList();
            }
        }

        return cities.Take(10).ToList();
    }

    private static void LoadCityData()
    {
        var client = new HttpClient();
        var response =  client.GetAsync(SourceDataUrl).Result;
        var stream = response.Content.ReadAsStreamAsync().Result;

        using (var gzipStream = new GZipStream(stream, CompressionMode.Decompress))
        using (var reader = new StreamReader(gzipStream))
        {
            var json = reader.ReadToEnd();

            var cities = JsonSerializer.Deserialize<List<City>>(json);
            if (cities != null)
            {
                Cities = cities;
            }
        }
    }
}
