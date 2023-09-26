using IDX_DPS.Utility.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupersetService.Models
{
    [Table("SS_BrokerSummary")]
    public class BrokerSummary
    {
        [Key]
        public int IDBrokerSummary { get; set; }

        [TransactionKey]
        public DateTime? Date { get; set; }

        [TransactionKey]
        public string? IDFirm { get; set; }

        public string? FirmName { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
    }
}
