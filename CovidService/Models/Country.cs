using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace CovidService.Models
{
    public class Country
    { 
        public Country()
        {
            States = new Dictionary<string, State>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, State> States { get; set; }
    }

}
