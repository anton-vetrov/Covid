using CovidService.Models;
using System.Collections.Generic;

namespace CovidService.Repositories
{
    public interface IRepository
    {
        public List<State> GetStates();
        public List<County> GetCounties();
    }
}
