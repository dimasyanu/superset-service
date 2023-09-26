namespace SupersetService.Models.Csv
{
    public class CsvRecapitulation
    {
        public DateTime? Date { get; set; }
        public string? IDInstrument { get; set; }
        public string? IDBoard { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Value { get; set; }
        public decimal? Frequency { get; set; }
    }
}
