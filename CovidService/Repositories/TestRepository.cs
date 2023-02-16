using System.Collections.Generic;
using System;
using CovidService.Models;
using System.Linq;

namespace CovidService.Repositories
{
    public class TestRepository : IRepository
    {
        public TestRepository()
        {
            using (var context = new CovidContext())
            {
                var country = new Country()
                {
                    Id = 840,
                    Name = "USA",
                    States = new List<State>() {
                        new State() {
                            Id = "Alabama",
                            Name = "Alabama",
                            Counties = new List<County>() {
                               new County() {
                                   Id   = 84001001,
                                   Name = "Autauga",
                                   Lat  = 32.53952745,
                                   Long = -86.64408227,
                                   Cases = new List<Case>() {
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 14),
                                           Count = 10
                                       },
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 13),
                                           Count = 15
                                       },
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 12),
                                           Count = 25
                                       }
                                   }

                               },
                               new County() {
                                   Id   = 84001003,
                                   Name = "Baldwin",
                                   Lat  = 30.72774991,
                                   Long = -87.72207058,
                                   Cases = new List<Case>() {
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 14),
                                           Count = 91
                                       },
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 13),
                                           Count = 101
                                       },
                                       new Case() {
                                           Date  = new DateTime(2023, 2, 12),
                                           Count = 121
                                       }
                                   }

                               }
                            }
                        }
                    }
                };

                context.Countries.AddRange(new List<Country>() { country });
                context.SaveChanges();
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
