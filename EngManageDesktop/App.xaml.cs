using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using EngManageDesktop.Views;
using Fool.Main;
using Fool.Services;
using Fool.TextManagement;
using Prism.Ioc;
using Prism.Logging;
using Prism.Modularity;
using Prism.Unity;
namespace EngManageDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(ILoggerFacade), typeof(HaiserLogger));
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var log = this.Container.Resolve<ILoggerFacade>();
            log.Log("System Startup", Category.Warn, Priority.High);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
           var log =  this.Container.Resolve<ILoggerFacade>();
           log.Log(e.Exception.Message, Category.Warn, Priority.High);
        }
        protected override Window CreateShell()
        {
            this.DispatcherUnhandledException += OnDispatcherUnhandledException;
            return Container.Resolve<MainWindow>();
        }
        //protected override IModuleCatalog CreateModuleCatalog()
        //{
        //    return new DirectoryModuleCatalog(){ ModulePath = "Modules"};
        //}
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            base.ConfigureModuleCatalog(moduleCatalog);
            moduleCatalog.AddModule(typeof(ServicesModule));
            moduleCatalog.AddModule(typeof(MainModule), "ServicesModule");
            moduleCatalog.AddModule(typeof(TextManagementModule), "ServicesModule", "MainModule"); 
        }
 
    }

}
