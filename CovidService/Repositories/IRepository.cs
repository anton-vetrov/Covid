using CovidService.Models;
using System.Collections.Generic;

namespace CovidService.Repositories
{
    public interface IRepository
    {
        public State GetState(string stateName);
        public List<County> GetCounties();
    }
}
