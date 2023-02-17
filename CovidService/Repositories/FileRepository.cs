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

namespace CovidService.Repositories
{
    public class FileRepository : IRepository
    {
        private class DistinctCountry
        {
            public DistinctCountry() 
            {
                States = new Dictionary<string, State>();
            }

            public int Id { get; set; }

            public string Name { get; set; }

            public Dictionary<string, State> States { get; set; }

            public Country ToCountry()
            {
                return new Country()
                {
                    Id = Id,
                    Name = Name,
                    States = States.Values.ToList()
                };
            }
        }
        /*
        public class DistinctState
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<County> Counties { get; set; }

            public State ToState()
            {
                return new State() 
                {
                    Id = Id,
                    Name = Name,
                    Counties = Counties
                };
            }
        }
        */

        public FileRepository(Stream stream)
        {
            using (var context = new CovidContext())
            {
                using (var reader = new StreamReader(stream))
                {
                    var distinctCountries = new Dictionary<int, DistinctCountry>();

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
                            var country = new DistinctCountry();
                            var state = new State()
                            {
                                Counties = new List<County>()
                            };
                            var county = new County(); // Supposed to be unique

                            var cases = new List<Case>();
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
                                        case "Province_State":
                                            state.Id = state.Name = csv[columnIndex];
                                            break;
                                        case "code3":
                                            country.Id = Int32.Parse(csv[columnIndex]);
                                            break;
                                        case "iso2":
                                            country.Name = csv[columnIndex];
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
                                    cases.Add(dateCase);
                                }
                            }

                            county.Cases = cases;

                            if (!distinctCountries.TryAdd(country.Id, country))
                                country = distinctCountries[country.Id];
                            if (!country.States.TryAdd(state.Id, state))
                                state = country.States[state.Id];
                            state.Counties.Add(county);
                        }


                    }

                    foreach (var distictCountry in distinctCountries.Values)
                    {
                        context.Countries.Add(distictCountry.ToCountry());
                    }

                    context.SaveChanges();
                }
            }
        }
        // TODO Add paging and selection
        public List<State> GetStates()
        {
            using (var context = new CovidContext())
            {
                return context.States.ToList();
            }
        }

        // TODO Add paging and selection
        public List<County> GetCounties()
        {
            using (var context = new CovidContext())
            {
                return context.Counties.ToList();
            }
        }
    }

}
