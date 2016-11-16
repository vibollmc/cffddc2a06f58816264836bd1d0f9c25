using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Convert
{
    public class Logging
    {
        private Logger _logger;
        public Logging()
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
            Error(x);
        }

        public void Error(string message, Exception x)
        {
            _logger.ErrorException(message, x);
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }


    }
}
