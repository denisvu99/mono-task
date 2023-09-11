using Ninject;
using Ninject.Modules;

namespace Mono.Services;
internal static class NinjectProvider{
    static StandardKernel _kernel;

    static public void Initialize(){

        _kernel = new StandardKernel(new NinjectModule[] {
            new AutoMapperModule(),
            new DataModule(),
            });
            
    }

    static public T Get<T>(){
        return _kernel.Get<T>();
    }
}