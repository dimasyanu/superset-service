using AutoMapper;
using SupersetService;
using SupersetService.Models;
using SupersetService.Workers;

namespace SuperSetService.Workers
{
    public class DailyTransactionWorker : BaseSupersetWorker
    {
        public DailyTransactionWorker(IConfiguration config, IMapper mapper, AppDbContext dbContext, ImportRepository importRepo) : base(config, mapper, dbContext, importRepo)
        {
        }
    }
}
