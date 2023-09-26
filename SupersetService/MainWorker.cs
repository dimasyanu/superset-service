using AutoMapper;
using SupersetService.Extensions;
using SupersetService.Models;
using SuperSetService.Contracts;
using SuperSetService.Workers;
using System.Diagnostics;
using System.IO.Compression;

namespace SupersetService
{
    public class MainWorker : BackgroundService
    {
        private readonly Serilog.ILogger _logger;
        private readonly IConfiguration _config;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;

        public MainWorker(Serilog.ILogger logger, IConfiguration config, IMapper mapper, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _mapper = mapper;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var startTime = DateTime.Now;
                try {
                    await WatchFolder();
                } catch (Exception ex) {
                    _logger.InternalErrors(ex);
                }

                var diffTime = (DateTime.Now - startTime).TotalMilliseconds;
                if (diffTime > 1000) continue;
                await Task.Delay(1000 - (int)diffTime, stoppingToken);
            }
        }

        protected async Task WatchFolder()
        {
            var watchDir = _config.AppSetting("WatchDir") ?? "WatchDir";
            if (!Directory.Exists(watchDir))
            {
                var dirInfo = Directory.CreateDirectory(watchDir);
                _logger.Information("Directory '" + dirInfo.FullName + "' does not exists. Successfully created.");
            }

            var rwebFolders = Directory.GetDirectories(watchDir);
            foreach (var rwebFolder in rwebFolders)
            {
                await WatchSingleRwebFolder(rwebFolder);
            }

            var files = Directory.GetFiles(watchDir).Where(x => x.ToLower().EndsWith(".zip"));
            if (!files.Any()) return;

            foreach (var file in files)
            {
                var fileNameSegments = file.Split('.') ?? Array.Empty<string>();
                var extractFolderName = string.Concat(fileNameSegments.Where((x, i) => i < fileNameSegments.Length - 1));
                if (Directory.Exists(extractFolderName))
                {
                    Directory.Delete(extractFolderName, true);
                }

                if (FileIsInUse(file)) {
                    await Task.Delay(1000);
                    return;
                }

                _logger.Information($"Extracting file '{file}'..");
                ZipFile.ExtractToDirectory(file, extractFolderName);
                _logger.Information($"'{file}' extracted successfully.");
                File.Delete(file);

                await WatchSingleRwebFolder(Path.Combine(watchDir, extractFolderName));
            }
        }

        protected async Task WatchSingleRwebFolder(string path)
        {
            IEnumerable<string> csvFiles = Directory.GetFiles(path);
            if (!csvFiles.Any())
            {
                try
                {
                    Directory.Delete(path);
                }
                catch (IOException e)
                {
                    Console.WriteLine(e.Message);
                }
                return;
            }

            using (var scope = _serviceProvider.CreateScope()) {
                var startTime = DateTime.Now;
                var startMsg = $"Watching folder '{path}'";
                _logger.Information(startMsg);
                Debug.WriteLine(startMsg);

                var tasks = new List<Task>();
                foreach (var csvFile in csvFiles) {
                    var filename = csvFile.ToLower().Split('\\').Last();
                    //if (!filename.StartsWith("a_")) return; // Test
                    var codeName = filename.Split('_').First();

                    var dbContext = scope.ServiceProvider.GetService<AppDbContext>();
                    var importRepo = scope.ServiceProvider.GetService<ImportRepository>();

                    if (dbContext == null)
                        throw new OperationCanceledException("DbContext service is not injected.");
                    if (importRepo == null)
                        throw new OperationCanceledException("ImportRepository service is not injected.");

                    ISupersetWorker? subWorker = null;
                    switch (codeName.ToLower()) {
                        case "a":
                            subWorker = new AllSummaryWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        case "b":
                            subWorker = new BrokerSummaryWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        case "d":
                            subWorker = new DailyTransactionWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        case "i":
                            subWorker = new IndexSummaryWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        case "r":
                            subWorker = new RecapitulationWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        case "s":
                            subWorker = new StockSummaryWorker(_config, _mapper, dbContext, importRepo);
                            break;
                        default: return;
                    }
                    if (subWorker == null) return;

                    tasks.Add(subWorker.ProcessCsv(csvFile));
                }
                await Task.WhenAll(tasks);

                var completeMsg = $"Completed, {decimal.Round((decimal)(DateTime.Now - startTime).TotalSeconds, 2)}s";
                _logger.Information(completeMsg);
                Debug.WriteLine(completeMsg);
            }

            csvFiles = Directory.GetFiles(path);
            if (csvFiles.Any()) return;
            try {
                Directory.Delete(path);
            }
            catch (IOException e) {
                Console.WriteLine(e.Message);
            }
        }

        private bool FileIsInUse(string path)
        {
            try {
                using FileStream stream = File.OpenRead(path);
                stream.Close();
            } catch (IOException) {
                return true;
            }

            return false;
        }
    }
}
