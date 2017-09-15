using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNuvem.UI.DataTable;
using XNuvem.UI.Navigation;

namespace XNuvem.UI
{
    public class UIModule : Module
    {
        protected override void Load(ContainerBuilder builder) {
            builder.RegisterType<DefaultMenuManager>().As<IMenuManager>().SingleInstance();
            builder.RegisterGeneric(typeof(JDataTable<>)).As(typeof(IJDataTable<>)).InstancePerDependency();
        }
    }
}
