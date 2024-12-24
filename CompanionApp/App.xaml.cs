using CompanionApp.Modules;
using CompanionApp.Views;
using MazeProject.ViewModels;
using MazeProject.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace CompanionApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialogWindow<SelectMapImageShell>("SelectMapImageShell");
            containerRegistry.RegisterDialog<SelectMapImageView, SelectMapImageViewModel>();
        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule<MainModule>();
            moduleCatalog.AddModule<SideTabModule>();
            moduleCatalog.AddModule<PresentationModule>();
            moduleCatalog.AddModule<ModulesModule>();
            moduleCatalog.AddModule<AtelierModule>();
            moduleCatalog.AddModule<DocModule>();



        }
    }
}
