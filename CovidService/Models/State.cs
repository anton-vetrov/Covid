using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CovidService.Models
{
    public class State
    {
        public State()
        {
            Counties = new Dictionary<string, County>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, County> Counties { get; set; }

    }
}
