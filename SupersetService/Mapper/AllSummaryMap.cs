using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using Serilog;
using SupersetService.Models.Csv;

namespace SupersetService.Mapper
{
    public class AllSummaryMap : ClassMap<CsvAllSummary>
    {
        public AllSummaryMap()
        {
            Map(x => x.TradeDate)
                .TypeConverter<DateTimeConverter>()
                .TypeConverterOption.Format("dd-MM-yyyy");
            Map(x => x.SecCode);
            Map(x => x.Board);
            Map(x => x.Instrument);
            Map(x => x.SecurityName);
            Map(x => x.Remarks);
            Map(x => x.PrevPrice);
            Map(x => x.OpenPrice);
            Map(x => x.FirstTrade);
            Map(x => x.High);
            Map(x => x.Low);
            Map(x => x.Close);
            Map(x => x.Change);
            Map(x => x.DaysVolume);
            Map(x => x.DaysValue);
            Map(x => x.NumTrade);
            Map(x => x.IndividualIndexVal);
            Map(x => x.CloseOffer);
            Map(x => x.OfferVolume);
            Map(x => x.Bid);
            Map(x => x.BidVolume);
            Map(x => x.ListedShares);
            Map(x => x.TradeableShares);
            Map(x => x.WeightForIndex);
            Map(x => x.ForeignSell);
            Map(x => x.ForeignBuy);
            Map(x => x.LastTradeDate);
            Map(x => x.NonRegVolume);
            Map(x => x.NonRegValue);
            Map(x => x.NonRegFreq);

        }
    }
}
