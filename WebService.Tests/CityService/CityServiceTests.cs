using CityPopulationWebService.Service.Interface;
using CityPopulationWebService.Extexsion;
using Microsoft.Extensions.DependencyInjection;

namespace WebService.Tests.CityService;

public sealed class CityServiceTests : IClassFixture<CityServiceTestFixture>
{
    public readonly ICityService cityService;

    public CityServiceTests(CityServiceTestFixture fixture)
    {
        this.cityService = fixture.ServiceProvider.GetRequiredService<ICityService>(); ;
    }

    [Fact(DisplayName = "Search by empty string")]
    public void SearchCities_ByEmptyStringShouldReturnFirstTenResults()
    {
        var result = this.cityService.Search();

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(10));
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }

    [Fact(DisplayName = "Search by name")]
    public void SearchCities_ByNameShouldReturnCorrectresult()
    {
        var result = this.cityService.Search("Sanaa");

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(1));
        Assert.All(result, c => c.Name.EqualsIgnoreCase("Sanaa"));
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }

    [Fact(DisplayName = "Search by prefix 'City of'")]
    public void SearchCities_ByPrefixShouldReturnCorrectResults()
    {
        var result = this.cityService.Search("City of");

        Assert.NotNull(result);
        Assert.True(result.Count <= 10);
        Assert.Contains(result, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of"));
    }

    [Fact(DisplayName = "Search by switch prefix 'of City'")]
    public void SearchCities_BySwithPrefixShouldReturnCorrectResults()
    {
        var result = this.cityService.Search("of City");

        Assert.NotNull(result);
        Assert.True(result.Count <= 10);
        Assert.Contains(result, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of"));
    }

    [Fact(DisplayName = "Search by prefix 'City of Bel'")]
    public void SearchCities_ByTwoPrefixShouldReturnCorrectResults()
    {
        var result = this.cityService.Search("City of Bel");

        Assert.NotNull(result);
        Assert.True(result.Count <= 10);
        Assert.Contains(result, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of") && c.Name.ContainsIgnoreCase("Bel"));
    }

    [Fact(DisplayName = "Search by Id")]
    public void SearchCities_ByIdShouldReturnOnlyOneResult()
    {
        var result = this.cityService.Search("14256");

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(1));
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }

    [Fact(DisplayName = "Search by Big Id")]
    public void SearchCities_ByBigIdShouldReturnOnlyOneResult()
    {
        var result = this.cityService.Search("7.16971E+6");

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(1));
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }

    [Fact(DisplayName = "Search by Ids")]
    public void SearchCities_ByIdsShouldReturnTwoResult()
    {
        var expectedIds = new List<double>() { 18918, 23814 };
        var result = this.cityService.Search("18918 23814");

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(2));
        foreach (var expectedId in expectedIds)
        {
            Assert.Contains(result, c => c.Id.Equals(expectedId));
        }
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }

    [Fact(DisplayName = "Search by Big Ids")]
    public void SearchCities_ByIdShouldReturn2Result()
    {
        var expectedIds = new List<double>() { 7.16971E+6, 7.12969E+7 };
        var result = this.cityService.Search("7.16971E+6 7.12969E+7");

        Assert.NotNull(result);
        Assert.True(result.Count.Equals(2));

        foreach (var expectedId in expectedIds)
        {
            Assert.Contains(result, c => c.Id.Equals(expectedId));
        }
        
        Assert.All(result, c => Assert.NotNull(c.Stat));
        Assert.All(result, c => Assert.True(c.Stat.Population > 0));
    }
}
