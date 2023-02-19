using CovidService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using System.Globalization;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CovidService.Services;
using System.Threading.Tasks;

namespace CovidService.Repositories
{
    public class FileRepository : IRepository
    {
        // TODO Hardcoded that to US country only
        private const int US = 840;

        Dictionary<int, Country> _countries;

        public FileRepository(Stream stream)
        {
            _countries = new Dictionary<int, Country>();

            using (var reader = new StreamReader(stream))
            {
                // CSV line length is less than 32767
                //string line = reader.ReadLine();
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Read();
                    csv.ReadHeader();

                    // 1. Find first Date
                    int firstDateIndex = -1;
                    var dates = new List<DateTime>();
                    for (int columnIndex = 0; columnIndex < csv.HeaderRecord.Length; columnIndex++)
                    {
                        DateTime date;
                        if (DateTime.TryParse(csv.HeaderRecord[columnIndex], out date))
                        {
                            if (firstDateIndex < 0)
                                firstDateIndex = columnIndex;

                            dates.Add(date);
                        }
                    }

                    if (firstDateIndex < 0)
                    {
                        throw (new ArgumentException("Date column has not been found"));
                    }

                    //2. Read line-by-line
                    while (csv.Read())
                    {
                        var country = new Country();
                        var state = new State();
                        var county = new County(); // Supposed to be unique

                        var cases = new SortedList<DateTime,Case>();
                        for (int columnIndex = 0; columnIndex < csv.HeaderRecord.Length; columnIndex++)
                        {
                            if (columnIndex < firstDateIndex)
                            // Read county & state
                            {
                                switch (csv.HeaderRecord[columnIndex])
                                {
                                    case "UID":
                                        county.Id = Int32.Parse(csv[columnIndex]);
                                        break;
                                    case "Admin2":
                                        county.Name = csv[columnIndex];
                                        break;
                                    case "Long_":
                                        county.Long = Double.Parse(csv[columnIndex]);
                                        break;
                                    case "Lat":
                                        county.Lat = Double.Parse(csv[columnIndex]);
                                        break;
                                    case "Combined_Key":
                                        county.CombinedKey = csv[columnIndex];
                                        break;
                                    case "Province_State":
                                        state.Id = state.Name = csv[columnIndex];
                                        break;
                                    case "code3":
                                        country.Id = Int32.Parse(csv[columnIndex]);
                                        break;
                                    case "iso2":
                                        country.Name = csv[columnIndex];
                                        break;
                                    case "Country_Region":
                                        country.CountryRegion = csv[columnIndex];
                                        break;


                                }
                            }
                            else
                            // Read cases
                            {
                                var dateCase = new Case()
                                {
                                    Date = dates[columnIndex - firstDateIndex],
                                    Count = Int32.Parse(csv[columnIndex])
                                };
                                cases.Add(dateCase.Date, dateCase);
                            }
                        }

                        if (!_countries.TryAdd(country.Id, country))
                            country = _countries[country.Id];
                        if (!country.States.TryAdd(state.Id, state))
                            state = country.States[state.Id];
                        if (!state.Counties.TryAdd(county.Name, county))
                            county = state.Counties[county.Name];

                        // TODO This will drop cases if there are duplicate line for the same country
                        county.Cases = cases;
                    }
                }
            }
        }
        public State GetState(string stateName)
        {
            State state;
            if (_countries[US].States.TryGetValue(stateName, out state))
                return state;

            return null;
        }

        public List<County> GetCounties()
        {
            var counties = new List<County>();

            List<State> states = _countries[US].States.Values.ToList();
            foreach (var state in states)
            {
                counties.AddRange(state.Counties.Values);
            }

            return counties;
        }

        public Task<State> GetStateAsync(string stateName)
        {
            State state;
            _countries[US].States.TryGetValue(stateName, out state);

            return Task.FromResult<State>(state);
        }

        public Task<List<County>> GetCountiesAsync()
        {
            var counties = new List<County>();

            List<State> states = _countries[US].States.Values.ToList();
            foreach (var state in states)
            {
                counties.AddRange(state.Counties.Values);
            }

            return Task.FromResult<List<County>>(counties);
        }
    }

}
