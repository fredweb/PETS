/****************************************************************************************
 *
 * Autor: George Santos
 * Copyright (c) 2016  
 *
 * Este código faz parte do Orchard e é livre para distribuição
 * 
 * 
/****************************************************************************************/


using System.Web.Hosting;

namespace XNuvem.FileSystems.AppData
{
    /// <summary>
    ///     Abstraction over the root location of "~/App_Data", mainly to enable
    ///     unit testing of AppDataFolder.
    /// </summary>
    public interface IAppDataFolderRoot : ISingletonDependency
    {
        /// <summary>
        ///     Virtual path of root ("~/App_Data")
        /// </summary>
        string RootPath { get; set; }

        /// <summary>
        ///     Physical path of root (typically: MapPath(RootPath))
        /// </summary>
        string RootFolder { get; set; }
    }

    public class AppDataFolderRoot : IAppDataFolderRoot
    {
        public string RootPath
        {
            get { return "~/App_Data"; }
            set { throw new System.NotImplementedException(); }
        }

        public string RootFolder
        {
            get { return HostingEnvironment.MapPath(RootPath); }
            set { throw new System.NotImplementedException(); }
        }
    }
}