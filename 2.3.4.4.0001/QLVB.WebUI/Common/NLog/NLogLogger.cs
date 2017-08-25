using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;

namespace QLVB.WebUI.Common.NLog
{
    public class NLogLogger : QLVB.Common.Logging.ILogger
    {
        private Logger _logger;

        public NLogLogger()
        {
            _logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }
        public void Error(Exception x)
        {
            Error(LogUtility.BuildExceptionMessage(x));
        }

        public void Error(string message, Exception x)
        {
            _logger.Error(message, x, null);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(LogUtility.BuildExceptionMessage(x));
        }
    }
}