namespace SupersetService.Models.Csv
{
    public class CsvBrokerSummary
    {
        public DateTime? Date { get; set; }
        public string? IDFirm { get; set; }
        public string? FirmName { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
    }
}
