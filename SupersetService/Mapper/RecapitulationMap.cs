using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SupersetService.Models.Csv;

namespace SupersetService.Mapper
{
    public class RecapitulationMap : ClassMap<CsvRecapitulation>
    {
        public RecapitulationMap()
        {
            Map(x => x.Date)
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("dd-MM-yyyy");

            Map(x => x.IDInstrument);
            Map(x => x.IDBoard);
            Map(x => x.Volume);
            Map(x => x.Value);
            Map(x => x.Frequency);
        }
    }
}
