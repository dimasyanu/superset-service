using AutoMapper;
using CsvHelper.Configuration;
using SupersetService.Extensions;
using SupersetService.Models;
using SuperSetService.Contracts;
using System.Globalization;

namespace SupersetService.Workers
{
    public class BaseSupersetWorker : ISupersetWorker
    {
        protected readonly IConfiguration _config;
        protected readonly IMapper _mapper;
        protected readonly AppDbContext _dbContext;
        protected readonly ImportRepository _importRepo;
        protected readonly CsvConfiguration _csvConfig;

        public BaseSupersetWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo)
        {
            _dbContext = dbContext;
            _config = config;
            _mapper = mapper;
            _importRepo = importRepo;
            _csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) {
                Delimiter = _config.AppSetting("CsvDelimiter") ?? "|",
                HasHeaderRecord = false
            };
        }

        public virtual async Task ProcessCsv(string csvFilePath)
        {
            try {
                File.Delete(csvFilePath);
                await Task.Delay(0);
            } catch(IOException e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
