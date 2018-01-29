using MvvmCross.Platform.IoC;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using BaseForms.Interfaces.IConnectors;
using BaseForms.Service.Connectors;

namespace BaseForms.Core
{
    public class CoreApp : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            //No es necesario registrar las instancias que termina como Service, se registra automaticamente
            //Mvx.LazyConstructAndRegisterSingleton<IWebClientService, WebClientService>();
            RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
        }
    }
}
