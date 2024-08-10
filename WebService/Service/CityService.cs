#nullable enable

using WebService.Extexsion;
using WebService.Model;
using WebService.Service.Interface;
using System.IO.Compression;
using System.Text.Json;
using System.Globalization;

namespace WebService.Service;

public sealed class CityService : ICityService
{

    private readonly static string SourceDataUrl = "http://bulk.openweathermap.org/sample/current.city.list.json.gz";

    private static IEnumerable<City> Cities = Array.Empty<City>();

    public CityService()
    {
        LoadCityData();
    }

    public IReadOnlyList<CityResponseModel> Search(CityFilterModel filters)
    {
        var cities = Cities;
        var page = Math.Max(filters.Page, 1);

        var searchTerms = new HashSet<string>() { };
        if (!string.IsNullOrEmpty(filters.Query))
        {
            searchTerms = filters.Query.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToHashSet();
        }
        var ids = searchTerms.Where(sc => double.TryParse(sc, out var id)).Select(sc => double.Parse(sc)).ToHashSet();

        if (ids.Count > 0)
        {
            cities = cities.Where(c => ids.Contains(c.Id));
        }
        else
        {
            foreach (var term in searchTerms)
            {
                if (string.IsNullOrEmpty(filters.Language))
                {
                    cities = cities.Where(c => c.Name.ContainsIgnoreCase(term) || c.Languages.Any(c => c.Value.ContainsIgnoreCase(term)));
                }
                else
                {
                    cities = cities.Where(c => c.Languages.TryGetValue(filters.Language, out var name) && name.ContainsIgnoreCase(term));
                }
            }
        }

        if (!string.IsNullOrEmpty(filters.Country))
        {
            cities = cities.Where(c => c.Country.EqualsIgnoreCase(filters.Country));
        }


        var response = cities
            .Skip((page - 1) * 10)
            .Take(10)
            .Select(c => new CityResponseModel
            {
                Id= c.Id,
                Country = c.Country,
                Population = c.Stat.Population,
                Name = TryFindKeyByValue(c.Languages, searchTerms, out var key) ? c.Languages[key] : c.Name,
                Language = string.IsNullOrEmpty(key) ? CultureInfo.GetCultureInfo("en").Name : key,
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
                Cities = cities.OrderBy(c => c.Id);
            }
        }
    }
}
