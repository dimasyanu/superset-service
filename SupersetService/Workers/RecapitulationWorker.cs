using AutoMapper;
using CsvHelper;
using SupersetService;
using SupersetService.Mapper;
using SupersetService.Models;
using SupersetService.Models.Csv;
using SupersetService.Workers;

namespace SuperSetService.Workers
{
    internal class RecapitulationWorker : BaseSupersetWorker
    {
        public RecapitulationWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo) : base(config, mapper, dbContext, importRepo)
        {
        }

        public override async Task ProcessCsv(string csvFilePath)
        {
            IEnumerable<Recapitulation> mappedItems;
            using (var streamReader = new StreamReader(csvFilePath)) {
                using var csvReader = new CsvReader(streamReader, _csvConfig);
                csvReader.Context.RegisterClassMap<RecapitulationMap>();
                var items = csvReader.GetRecords<CsvRecapitulation>().ToList();
                mappedItems = _mapper.Map<IEnumerable<Recapitulation>>(items);
            }
            await _importRepo.BatchImport(mappedItems);
            File.Delete(csvFilePath);
        }
    }
}
