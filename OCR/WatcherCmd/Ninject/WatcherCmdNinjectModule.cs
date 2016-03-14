using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatcherCmd.Files;

namespace WatcherCmd.Ninject
{
    class WatcherCmdNinjectModule : NinjectModule
    {

        public override void Load()
        {
            Bind<Watcher>().To<Watcher>();
        }

        public static T GetDependency<T>()
        {
            return new StandardKernel(new WatcherCmdNinjectModule()).Get<T>();
        }

    }
}
