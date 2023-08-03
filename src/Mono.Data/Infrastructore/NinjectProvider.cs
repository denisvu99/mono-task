using Ninject;
using Ninject.Modules;

namespace Mono.Data;
public static class NinjectProvider{
    static StandardKernel _kernel;

    static public void Initialize(){
        _kernel = new StandardKernel(new NinjectModule[] {
            new DataModule(), 
            new AutoMapperModule()
        });
    }

    static public T Get<T>(){
        return _kernel.Get<T>();
    }
}