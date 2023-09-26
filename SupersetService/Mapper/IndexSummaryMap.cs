using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SupersetService.Models.Csv;

namespace SupersetService.Mapper
{
    public class IndexSummaryMap : ClassMap<CsvIndexSummary>
    {
        public IndexSummaryMap()
        {
            Map(x => x.Date)
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("dd-MM-yyyy");
            Map(x => x.IndexCode);
            Map(x => x.Previous);
            Map(x => x.Highest);
            Map(x => x.Lowest);
            Map(x => x.Close);
            Map(x => x.NumberOfStock);
            Map(x => x.Change);
            Map(x => x.Volume);
            Map(x => x.Value);
            Map(x => x.Frequency);
            Map(x => x.MarketCapital);
        }
    }
}
