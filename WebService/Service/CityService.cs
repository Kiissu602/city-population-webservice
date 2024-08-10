#nullable enable

using WebService.Extexsion;
using WebService.JsonConverter;
using WebService.Model;
using WebService.Service.Interface;
using System.IO.Compression;
using System.Text.Json;
namespace WebService.Service;

public sealed class CityService : ICityService
{

    private readonly static string SourceDataUrl = "http://bulk.openweathermap.org/sample/current.city.list.json.gz";

    private static IReadOnlyList<City> Cities = Array.Empty<City>();

    public CityService()
    {
        LoadCityData();
    }

    public IReadOnlyList<CityResponseModel> Search(string? name = "")
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
                cities = cities.Where(c => c.Name.ContainsIgnoreCase(term) || c.Languages.Any(c => c.Value.ContainsIgnoreCase(term))).ToList();
            }
        }

        var response = cities.Take(10).Select(c => new CityResponseModel
        {
            Id= c.Id,
            Country = c.Country,
            Population = c.Stat.Population,
            Name = TryFindKeyByValue(c.Languages, searchTerms, out var key) ? c.Languages[key] : c.Name,
            Language = key
        })
        .OrderBy(c => c.Name)
        .ToList();

        return response;

        static bool TryFindKeyByValue(IReadOnlyDictionary<string, string> dictionary, HashSet<string> values, out string key)
        {
            foreach (var kvp in dictionary)
            {
                if (values.Any(v => v.ContainsIgnoreCase(kvp.Value)))
                {
                    key = kvp.Key;
                    return true;
                }
            }

            key = string.Empty;
            return false;
        }
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
            var options = new JsonSerializerOptions
            {
                Converters = { new LanguagesConverter() }
            };

            var cities = JsonSerializer.Deserialize<List<City>>(json, options);
            if (cities != null)
            {
                Cities = cities.OrderBy(c => c.Id).ToList();
            }
        }
    }
}
