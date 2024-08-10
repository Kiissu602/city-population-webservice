using WebService.Service.Interface;
using WebService.Extexsion;
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
        var cities = this.cityService.Search(new Model.CityFilterModel() { });

        Assert.NotNull(cities);
        Assert.True(cities.PageSize.Equals(10));
        Assert.All(cities.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by empty string but imppossible page")]
    public void SearchCities_ByEmptyStringImpossiblePageShouldReturnFirstTenResults()
    {
        var cities = this.cityService.Search(new Model.CityFilterModel() 
        {
            Page = 99999999,
        });

        Assert.NotNull(cities);
        Assert.True(cities.PageSize.Equals(0));
    }

    [Fact(DisplayName = "Search by name")]
    public void SearchCities_ByNameShouldReturnCorrectresult()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "Sanaa"
        };

        var cities = this.cityService.Search(filter);

        Assert.NotNull(cities);
        Assert.True(cities.PageSize.Equals(1));
        Assert.All(cities.Response, c => c.Name.EqualsIgnoreCase(filter.Query));
        Assert.All(cities.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by various name 'پیرانشهر'")]
    public void SearchCities_ByVariousNameShouldReturnCorrectresult()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "پیرانشهر"
        };
        var cities = this.cityService.Search(filter);

        Assert.NotNull(cities);
        Assert.True(cities.PageSize.Equals(1));
        Assert.All(cities.Response, c => c.Name.EqualsIgnoreCase(filter.Query));
        Assert.All(cities.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by various name 'پیرانشهر' and wrong language")]
    public void SearchCities_ByVariousNameWrongLanguageShouldReturnCorrectresult()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "پیرانشهر",
            Language = "en"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize.Equals(0));
    }

    [Fact(DisplayName = "Search by country code")]
    public void SearchCities_CountryCodeShouldReturnCorrectresult()
    {
        var filter = new Model.CityFilterModel()
        {
            Country = "IR"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize <= 10);
        Assert.All(result.Response, c => c.Country.EqualsIgnoreCase("IR"));
    }

    [Fact(DisplayName = "Search by prefix 'City of'")]
    public void SearchCities_ByPrefixShouldReturnCorrectResults()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "City of"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize <= 10);
        Assert.Contains(result.Response, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of"));
    }

    [Fact(DisplayName = "Search by switch prefix 'of City'")]
    public void SearchCities_BySwithPrefixShouldReturnCorrectResults()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "of City"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize <= 10);
        Assert.Contains(result.Response, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of"));
    }

    [Fact(DisplayName = "Search by prefix 'City of Bel'")]
    public void SearchCities_ByTwoPrefixShouldReturnCorrectResults()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "City of Bel"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize <= 10);
        Assert.Contains(result.Response, c => c.Name.ContainsIgnoreCase("City") && c.Name.ContainsIgnoreCase("of") && c.Name.ContainsIgnoreCase("Bel"));
    }

    [Fact(DisplayName = "Search by Id")]
    public void SearchCities_ByIdShouldReturnOnlyOneResult()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "14256"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize.Equals(1));
        Assert.All(result.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by Big Id")]
    public void SearchCities_ByBigIdShouldReturnOnlyOneResult()
    {
        var filter = new Model.CityFilterModel()
        {
            Query = "7.16971E+6"
        };
        var result = this.cityService.Search(filter);


        Assert.NotNull(result);
        Assert.True(result.PageSize.Equals(1));
        Assert.All(result.Response, c => Assert.True(c.Id.Equals(7.16971E+6)));
        Assert.All(result.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by Ids")]
    public void SearchCities_ByIdsShouldReturnTwoResult()
    {
        var expectedIds = new List<double>() { 18918, 23814 };
        var filter = new Model.CityFilterModel()
        {
            Query = "18918 23814"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.PageSize.Equals(2));
        foreach (var expectedId in expectedIds)
        {
            Assert.Contains(result.Response, c => c.Id.Equals(expectedId));
        }

        Assert.All(result.Response, c => Assert.True(c.Population > 0));
    }

    [Fact(DisplayName = "Search by Big Ids")]
    public void SearchCities_ByIdShouldReturn2Result()
    {
        var expectedIds = new List<double>() { 7.16971E+6, 7.12969E+7 };
        var filter = new Model.CityFilterModel()
        {
            Query = "7.16971E+6 7.12969E+7"
        };
        var result = this.cityService.Search(filter);

        Assert.NotNull(result);
        Assert.True(result.Response.Count.Equals(2));

        foreach (var expectedId in expectedIds)
        {
            Assert.Contains(result.Response, c => c.Id.Equals(expectedId));
        }
        
        Assert.All(result.Response, c => Assert.NotNull(c));
        Assert.All(result.Response, c => Assert.True(c.Population > 0));
    }
}
