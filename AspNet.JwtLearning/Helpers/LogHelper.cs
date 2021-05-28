using log4net;

namespace AspNet.JwtLearning.Helpers
{
    public class LogHelper
    {
        private static readonly ILog log = LogManager.GetLogger("LogHelper");

        public static void WriteLog(string msg)
        {
            log.Info(msg);
        }
    }
}