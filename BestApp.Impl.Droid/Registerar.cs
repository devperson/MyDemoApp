using BestApp.Abstraction.Main.PlatformServices;
using BestApp.Abstraction.Main.UI;
using BestApp.Impl.Droid.PlatformServices;
using BestApp.Impl.Droid.PlatformServices.Video;
using BestApp.Impl.Droid.UI;
using BestApp.Impl.PlatformServices;
using Common.Abstrtactions;
using DryIoc;
using Mapster;

namespace BestApp.Impl.Droid
{
    public static class Registerar
    {
        public static void RegisterTypes(IContainer container, TypeAdapterConfig mapperConfig)
        {
            container.Register<IPreferencesService, PreferencesService>(Reuse.Singleton);
            container.Register<IDirectoryService, DirectoryService>(Reuse.Singleton);
            container.Register<IPopupAlert, MockPopup>(Reuse.Singleton);
            container.Register<IPlatformErrorService, PlatformErrorService>(Reuse.Singleton);
            container.Register<IAlertDialogService, DroidAlertDialogService>(Reuse.Singleton);
            container.Register<IDevice, DeviceInfoService>(Reuse.Singleton);
            container.Register<IDeviceThreadService, DeviceThreadService>(Reuse.Singleton);
            container.Register<IResizeImageService, ResizeImageService>(Reuse.Singleton);
            container.Register<IVideoService, VideoService>(Reuse.Singleton);
            container.Register<IZipService, ZipService>(Reuse.Singleton);
            container.Register<IShareFileService, ShareFileService>(Reuse.Singleton);
        }
    }
}
