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
    public class ModulesModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var region = containerProvider.Resolve<IRegionManager>();
            region.RegisterViewWithRegion("ModulesRegion", typeof(ModulesView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }
    }
}
