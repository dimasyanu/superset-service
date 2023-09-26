using AutoMapper;
using CsvHelper;
using SupersetService;
using SupersetService.Mapper;
using SupersetService.Models;
using SupersetService.Models.Csv;
using SupersetService.Workers;

namespace SuperSetService.Workers
{
    public class IndexSummaryWorker : BaseSupersetWorker
    {
        public IndexSummaryWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo) : base(config, mapper, dbContext, importRepo)
        {

        }

        public override async Task ProcessCsv(string csvFilePath)
        {
            IEnumerable<IndexSummary> mappedItems;
            using (var streamReader = new StreamReader(csvFilePath)) {
                using var csvReader = new CsvReader(streamReader, _csvConfig);
                csvReader.Context.RegisterClassMap<IndexSummaryMap>();
                var items = csvReader.GetRecords<CsvIndexSummary>().ToList();
                mappedItems = _mapper.Map<IEnumerable<IndexSummary>>(items);
            }
            await _importRepo.BatchImport(mappedItems);
            File.Delete(csvFilePath);
        }
    }
}
