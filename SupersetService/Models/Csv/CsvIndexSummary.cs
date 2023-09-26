namespace SupersetService.Models.Csv
{
    public class CsvIndexSummary
    {
        public DateTime? Date { get; set; }
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
