using CompanionApp.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanionApp.Modules
{
    public class SideTabModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();
            region.RegisterViewWithRegion("SideTabRegion", typeof(SideTabView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
