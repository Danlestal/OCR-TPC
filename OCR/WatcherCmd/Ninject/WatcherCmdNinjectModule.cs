using Ninject;
using Ninject.Modules;
using WatcherCmd.Configuration;
using WatcherCmd.Files;
using WatcherCmd.Files.Interface;
using WatcherCmd.Jobs;

namespace WatcherCmd.Ninject
{
    class WatcherCmdNinjectModule : NinjectModule
    {

        public override void Load()
        {
            Bind<ICronDispatcher>().To<Dispatcher>();
            Bind<IConfigurationProvider>().To<ConfigurationProvider>();
            Bind<IWatcher>().To<Watcher>();
            Bind<IManager>().To<Manager>();
        }

        public static T GetDependency<T>()
        {
            return new StandardKernel(new WatcherCmdNinjectModule()).Get<T>();
        }

    }
}
