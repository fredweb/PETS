using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XNuvem.UI.Navigation;

namespace XNuvem.Producao
{
    public class ModuleMenu : IMenuProvider
    {
        public void BuildMenu(MenuBuilder builder) {
            builder.AddGroup("5", "Relatórios de produção");
            builder.AddAction("5.1", "Cadastro de Metas", "MetaProducaoList", "Config", new { area = "XNuvem.Producao" });
            builder.AddAction("5.2", "Relatorio de producao", "Produtividade", "Report", new { area = "XNuvem.Producao" });
            builder.AddAction("5.3", "Relatorio de producao - WEB", "ProducaoWeb", "Report", new { area = "XNuvem.Producao" });
        }
    }
}