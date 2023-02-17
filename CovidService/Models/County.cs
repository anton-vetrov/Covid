using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CovidService.Models
{
    public class County
    {
        // This is UID
        public int Id { get; set; }

        public string Name { get; set; }
        public string CombinedKey { get; set; }

        // TODO Combined Key
        public double Lat { get; set; }
        public double Long { get; set; }

        // Should be sorted by date in case of wrong sort order in CSV
        public SortedList<DateTime,Case> Cases { get; set; }
    }
}
