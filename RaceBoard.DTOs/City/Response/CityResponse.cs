﻿using RaceBoard.DTOs.Country.Response;

namespace RaceBoard.DTOs.City.Response
{
    public class CityResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CountryResponse Country {  get; set; }
    }
}