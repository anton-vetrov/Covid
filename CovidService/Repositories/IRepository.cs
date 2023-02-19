﻿using CovidService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CovidService.Repositories
{
    public interface IRepository
    {
        public State GetState(string stateName);
        public List<County> GetCounties();

        public Task<State> GetStateAsync(string stateName);
        public Task<List<County>> GetCountiesAsync();

    }
}
