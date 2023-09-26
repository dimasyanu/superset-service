using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SupersetService.Models.Csv;

namespace SupersetService.Mapper
{
    public class BrokerSummaryMap : ClassMap<CsvBrokerSummary>
    {
        public BrokerSummaryMap()
        {
            Map(x => x.Date)
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("dd-MM-yyyy");
            Map(x => x.IDFirm);
            Map(x => x.FirmName);
            Map(x => x.Volume);
            Map(x => x.Value);
            Map(x => x.Frequency);
        }
    }
}
