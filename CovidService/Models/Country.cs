using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CovidService.Models
{
    public class Country
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public List<State> States { get; set; }
    }
}
