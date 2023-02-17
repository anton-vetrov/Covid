using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CovidService.Models
{
    public class County
    {
        // This is UID
        public int Id { get; set; }

        public string Name { get; set; }

        // TODO Combined Key
        public double Lat { get; set; }
        public double Long { get; set; }

        public List<Case> Cases { get; set; }
    }
}
