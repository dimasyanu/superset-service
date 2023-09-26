namespace SuperSetService.Contracts
{
    internal interface ISupersetWorker
    {
        public Task ProcessCsv(string csvFilePath);
    }
}
