﻿using WebService.Model;

namespace WebService.Service.Interface;

public interface ICityService
{
    public IReadOnlyList<CityResponseModel> Search(string? name = "", int? page = 0);
}
