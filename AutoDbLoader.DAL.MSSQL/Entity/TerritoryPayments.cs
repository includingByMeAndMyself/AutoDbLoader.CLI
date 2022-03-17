using System.ComponentModel.DataAnnotations;

namespace AutoDbLoader.DAL.MSSQL.Entity
{
    public class TerritoryPayments
    {
        [Key]
        public string PersonalAccount { get; set; }
        public string Payer { get; set; }
        public string Address { get; set; }
        public string IndicationPeriod { get; set; }
        public string Debt { get; set; }
        public string Alias { get; set; }
        public string INN { get; set; }
        public string PaymentAccount { get; set; }
    }
}
