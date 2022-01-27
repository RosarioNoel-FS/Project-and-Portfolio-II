using System;
namespace TaxEz.Models
{
    public class TaxData
    {
        public decimal PreTaxAmount { get; set; }
        public decimal TakeHomeAmount { get; set; }
        public decimal Deducted { get; set; }
        public string JobName { get; set; }
        public int HoursWorked { get; set; }
    }
}
