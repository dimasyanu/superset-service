using CsvHelper.Configuration.Attributes;

namespace SupersetService.Models.Csv
{
    public class CsvStockSummary
    {
        public DateTime? Date { get; set; }
        public string? StockCode { get; set; }
        public string? StockName { get; set; }
        public string? Remarks { get; set; }
        public decimal? Previous { get; set; }
        public decimal? OpenPrice { get; set; }
        public decimal? FirstTrade { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }
        public decimal? Change { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
        public decimal? IndexIndividual { get; set; }
        public decimal? Offer { get; set; }
        public decimal? OfferVolume { get; set; }
        public decimal? Bid { get; set; }
        public decimal? BidVolume { get; set; }
        public decimal? ListedShares { get; set; }
        public decimal? TradebleShares { get; set; }
        public decimal? WeightForIndex { get; set; }
        public decimal? ForeignSell { get; set; }
        public decimal? ForeignBuy { get; set; }
        public string? DelistingDate { get; set; }
        public decimal? NonRegularVolume { get; set; }
        public decimal? NonRegularValue { get; set; }
        public decimal? NonRegularFrequency { get; set; }
    }
}
