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

namespace XNuvem.Vendas.Services.Models
{
    public class CompanySettings
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public string ServerName { get; set; }
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MinPoolSize { get; set; }
        public string MaxPoolSize { get; set; }
        public string ApplicationDB { get; set; }
        public override bool Equals(object obj) {
            if(!(obj is CompanySettings))
                return base.Equals(obj);
            var cmp = (obj as CompanySettings);
            return cmp.Name == this.Name &&
                cmp.ServerName == this.ServerName &&
                cmp.CompanyDB == this.CompanyDB &&
                cmp.UserName == this.UserName &&
                cmp.Password == this.Password &&
                cmp.MinPoolSize == this.MinPoolSize &&
                cmp.MaxPoolSize == this.MaxPoolSize &&
                cmp.ApplicationDB == this.ApplicationDB;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}