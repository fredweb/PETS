/****************************************************************************************
 *
 * Autor: Marvin Mendes
 * Copyright (c) 2016  
 * 
/****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.Security.Permissions;

namespace XNuvem.Environment.Configuration
{
    public interface IShellSettingsManager
    {
        ShellSettings GetSettings();
        void StoreSettings(ShellSettings settings);
        IEnumerable<Permission> GetPermissions();

        bool HasConfigurationFile();
    }
}
