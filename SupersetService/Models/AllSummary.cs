using IDX_DPS.Utility.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SupersetService.Models
{
    [Table("SS_AllSummary")]
    public class AllSummary
    {
        [Key]
        public int Id { get; set; }

        [TransactionKey]
        public DateTime? TradeDate { get; set; }
        
        [TransactionKey]
        public string? SecCode { get; set; }
        
        [TransactionKey]
        public string? Board { get; set; }

        [TransactionKey]
        public string? Instrument { get; set; }
        
        public string? SecurityName { get; set; }
        public string? Remarks { get; set; }
        public decimal? PrevPrice { get; set; }
        public decimal? OpenPrice { get; set; }
        public decimal? FirstTrade { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? Change { get; set; }
        public decimal? DaysVolume { get; set; }
        public decimal? DaysValue { get; set; }
        public decimal? NumTrade { get; set; }
        public decimal? IndividualIndexVal { get; set; }
        public decimal? CloseOffer { get; set; }
        public decimal? OfferVolume { get; set; }
        public decimal? Bid { get; set; }
        public decimal? BidVolume { get; set; }
        public decimal? ListedShares { get; set; }
        public decimal? TradeableShares { get; set; }
        public decimal? WeightForIndex { get; set; }
        public decimal? ForeignSell { get; set; }
        public decimal? ForeignBuy { get; set; }
        public DateTime? LastTradeDate { get; set; }
        public decimal? NonRegVolume { get; set; }
        public decimal? NonRegValue { get; set; }
        public decimal? NonRegFreq { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
