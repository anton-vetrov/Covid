using Microsoft.VisualBasic;
using System;

namespace CovidService.Models
{
    public class Case
    { 
        // Unix time / 100 since it is 24*60*60
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
}
