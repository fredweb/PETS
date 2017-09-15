/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.Environment.Configuration;
using XNuvem.Vendas.Services.Models;

namespace XNuvem.Vendas.Services
{
    public class DefaultB1ConfigurationManager : IB1ConfigurationManager
    {
        private readonly IShellSettingsManager _shellSettingsManager;

        public DefaultB1ConfigurationManager(IShellSettingsManager shellSettingsManager) {
            _shellSettingsManager = shellSettingsManager;
        }

        public CompanySettings GetSettings() {
            string name, connectionString, serverName, companyDB, userName, password, minPoolSize, maxPoolSize, applicationDB;
            var settings = _shellSettingsManager.GetSettings();
            settings.GeneralSettings.TryGetValue("SAPCompanyName", out name);
            settings.GeneralSettings.TryGetValue("SAPConnectionString", out connectionString);
            settings.GeneralSettings.TryGetValue("SAPServerName", out serverName);
            settings.GeneralSettings.TryGetValue("SAPCompanyDB", out companyDB);
            settings.GeneralSettings.TryGetValue("SAPUserName", out userName);
            settings.GeneralSettings.TryGetValue("SAPPassword", out password);
            settings.GeneralSettings.TryGetValue("SAPMinPool", out minPoolSize);
            settings.GeneralSettings.TryGetValue("SAPMaxPool", out maxPoolSize);
            settings.GeneralSettings.TryGetValue("ApplicationDB", out applicationDB);
            var result = new CompanySettings {
                Name = name ?? "",
                ConnectionString = connectionString ?? "",
                ServerName = serverName ?? "",
                CompanyDB = companyDB ?? "",
                UserName = userName ?? "",
                Password = password ?? "",
                MinPoolSize = minPoolSize ?? "1",
                MaxPoolSize = maxPoolSize ?? "25",
                ApplicationDB = applicationDB
            };
            return result;
        }

        public void StoreSettings(CompanySettings settings) {
            var gs = _shellSettingsManager.GetSettings();
            gs.GeneralSettings["SAPCompanyName"] = settings.Name;
            gs.GeneralSettings["SAPConnectionString"] = settings.ConnectionString;
            gs.GeneralSettings["SAPServerName"] = settings.ServerName;
            gs.GeneralSettings["SAPCompanyDB"] = settings.CompanyDB;
            gs.GeneralSettings["SAPUserName"] = settings.UserName;
            gs.GeneralSettings["SAPPassword"] = settings.Password;
            gs.GeneralSettings["SAPMinPool"] = settings.MinPoolSize;
            gs.GeneralSettings["SAPMaxPool"] = settings.MaxPoolSize;
            gs.GeneralSettings["ApplicationDB"] = settings.ApplicationDB;
            _shellSettingsManager.StoreSettings(gs);
        }
    }
}