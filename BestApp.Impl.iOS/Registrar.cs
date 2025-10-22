

using DryIoc;
using Mapster;

namespace BestApp.Impl.iOS
{
    public static class Registrar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            BestApp.Impl.Cross.Registrar.RegisterTypes(container, mapperConfig);
            global::Base.Impl.iOS.Registrar.RegisterTypes(container);
        }
    }
}
