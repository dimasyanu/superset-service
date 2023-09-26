using AutoMapper;
using CsvHelper;
using SupersetService;
using SupersetService.Mapper;
using SupersetService.Models;
using SupersetService.Models.Csv;
using SupersetService.Workers;

namespace SuperSetService.Workers
{
    internal class StockSummaryWorker : BaseSupersetWorker
    {
        public StockSummaryWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo) : base(config, mapper, dbContext, importRepo) { }

        public override async Task ProcessCsv(string csvFilePath)
        {
            IEnumerable<StockSummary> mappedItems;
            using (var streamReader = new StreamReader(csvFilePath)) {
                using var csvReader = new CsvReader(streamReader, _csvConfig);
                csvReader.Context.RegisterClassMap<StockSummaryMap>();
                var items = csvReader.GetRecords<CsvStockSummary>().ToList();
                mappedItems = _mapper.Map<IEnumerable<StockSummary>>(items);
            }
            await _importRepo.BatchImport(mappedItems);
            File.Delete(csvFilePath);
        }
    }
}
