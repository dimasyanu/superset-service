using AutoMapper;
using CsvHelper;
using SupersetService;
using SupersetService.Mapper;
using SupersetService.Models;
using SupersetService.Models.Csv;
using SupersetService.Workers;

namespace SuperSetService.Workers
{
    internal class AllSummaryWorker : BaseSupersetWorker
    {
        public AllSummaryWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo) : base(config, mapper, dbContext, importRepo)
        {
        }

        public override async Task ProcessCsv(string csvFilePath)
        {
            IEnumerable<AllSummary> mappedItems;
            using (var streamReader = new StreamReader(csvFilePath)) {
                using var csvReader = new CsvReader(streamReader, _csvConfig);
                csvReader.Context.RegisterClassMap<AllSummaryMap>();
                var items = csvReader.GetRecords<CsvAllSummary>().ToList();
                mappedItems = _mapper.Map<IEnumerable<AllSummary>>(items);
            }
            await _importRepo.BatchImport(mappedItems);
            File.Delete(csvFilePath);
        }
    }
}
