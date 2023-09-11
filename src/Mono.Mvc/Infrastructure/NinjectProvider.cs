using Ninject;
using Ninject.Modules;

namespace Mono.Mvc;
internal static class NinjectProvider{
    static StandardKernel _kernel;

    static public void Initialize(){
        _kernel = new StandardKernel(new ServicesModule());
    }

    static public T Get<T>(){
        return _kernel.Get<T>();
    }
}