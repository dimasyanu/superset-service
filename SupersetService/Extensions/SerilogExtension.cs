namespace SupersetService.Extensions
{
    public static class SerilogExtension
    {
        public static void InternalErrors(this Serilog.ILogger logger, Exception e)
        {
            if (e.InnerException != null) {
                logger.InternalErrors(e.InnerException);
            }
            logger.Error(e.Message);
            logger.Error(e.StackTrace);
        }
    }
}
