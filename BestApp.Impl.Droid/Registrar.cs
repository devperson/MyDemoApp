using DryIoc;
using Mapster;
namespace BestApp.Impl.Droid
{
    public static class Registrar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            BestApp.Impl.Cross.Registrar.RegisterTypes(container, mapperConfig);
            Base.Impl.Droid.Registrar.RegisterTypes(container);            
        }
    }
}