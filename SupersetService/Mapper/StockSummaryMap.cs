using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using SupersetService.Models.Csv;

namespace SupersetService.Mapper
{
    public class StockSummaryMap : ClassMap<CsvStockSummary>
    {
        public StockSummaryMap()
        {
            Map(x => x.Date)
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("dd-MM-yyyy");

            Map(x => x.StockCode);
            Map(x => x.StockName);
            Map(x => x.Remarks);
            Map(x => x.Previous);
            Map(x => x.OpenPrice);
            Map(x => x.FirstTrade);
            Map(x => x.High);
            Map(x => x.Low);
            Map(x => x.Close);
            Map(x => x.Change);
            Map(x => x.Volume);
            Map(x => x.Value);
            Map(x => x.Frequency);
            Map(x => x.IndexIndividual);
            Map(x => x.Offer);
            Map(x => x.OfferVolume);
            Map(x => x.Bid);
            Map(x => x.BidVolume);
            Map(x => x.ListedShares);
            Map(x => x.TradebleShares);
            Map(x => x.WeightForIndex);
            Map(x => x.ForeignSell);
            Map(x => x.ForeignBuy);
            Map(x => x.DelistingDate);
            Map(x => x.NonRegularVolume);
            Map(x => x.NonRegularValue);
            Map(x => x.NonRegularFrequency);
        }
    }
}
