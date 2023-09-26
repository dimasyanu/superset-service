using IDX_DPS.Utility.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupersetService.Models
{
    [Table("SS_IndexSummary")]
    public class IndexSummary
    {
        [Key]
        public int IndexSummaryID { get; set; }

        [TransactionKey]
        public DateTime? Date { get; set; }

        [TransactionKey]
        public string? IndexCode { get; set; }

        public decimal? Previous { get; set; }
        public decimal? Highest { get; set; }
        public decimal? Lowest { get; set; }
        public decimal? Close { get; set; }
        public decimal? NumberOfStock { get; set; }
        public decimal? Change { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
        public decimal? MarketCapital { get; set; }
    }
}
