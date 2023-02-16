using CovidService.Repositories;
using System;

namespace CovidService.Services
{
    public class CountyService : ICountyService
    {
        private IRepository _repository;
        public CountyService(IRepository repository)
        {
            _repository = repository;
        }
        public CountySummary GetSummary()
        {
            var summary = new CountySummary();

            _repository.GetCounties();

            return summary;
        }
        public Breakdown GetBreakDown()
        {
            throw (new NotImplementedException());
        }
        public Rate GetRate()
        {
            throw (new NotImplementedException());
        }

    }
}
