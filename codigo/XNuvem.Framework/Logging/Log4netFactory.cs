/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 *
 * Este código faz parte do Orchard e é livre para distribuição
 * 
 * 
/****************************************************************************************/

using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XNuvem.Logging
{
    public class Log4netFactory : ILoggerFactory
    {
        private static bool _isFileWatched = false;
        public ILogger CreateLogger(Type type) {
            if (!_isFileWatched) {
                var configInfo = GetFileInfo();
                if (null != configInfo) {                    
                    XmlConfigurator.ConfigureAndWatch(configInfo);
                    _isFileWatched = true;
                }                
            }
            return new Log4netLogger(LogManager.GetLogger(type), this);
        }

        public FileInfo GetFileInfo() {
            string fileName = ConfigurationManager.AppSettings["log4net.Config"];
            if (String.IsNullOrEmpty(fileName))
                return null;
            string currentPath = "";
            if (null != HttpContext.Current) {
                currentPath = HttpContext.Current.Server.MapPath("~/");
            }
            else {
                currentPath = Directory.GetCurrentDirectory();
            }

            string fileFullName = Path.Combine(currentPath, fileName);
            if (File.Exists(fileFullName)) {
                return new FileInfo(fileFullName);
            }
            else {
                return null;
            }
        }
    }
}
