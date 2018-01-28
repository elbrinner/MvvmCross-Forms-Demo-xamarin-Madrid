<h1>Xamarin Forms con MvvmCross 5.6</h1>
<h2>Demo para Xamarin Madrid</h2>
<p>Tutorial de como crear una aplicación con Xamarin Forms y MvvmCross</p>

<p>En la carpeta inicio, se encuentra una solución con MvvmCross configurado, en la carpeta fin se encuentra el ejemplo completo del tutorial.</p>


<h3>Paso 1 - Crear un proyecto Xamarin Forms</h3>


<p>Abra visual studio y selecciona la opción crear nueva solución => Multiplataforma => Aplicación de Forms en blanco.</p>

<img src="/Img/img1.png"/> 

<img src="/Img/img2.png"/>

<h3>Paso 2 - Configurar MvvmCross</h3>

<img src="/Img/img3.png"/>

<ol>
 <li>Agregue <strong>.Core </strong>al nombre del proyecto PCL.</li>
 <li>Elimine la página que fue creada por los formularios de Xamarin.</li>
 <li>Agregue el paquete MvvmCross Forms StarterPack a los proyectos específicos de la plataforma y .Core</li>
 <li>En Android: elimine MainActivity.cs.</li>
 <li>En los proyectos específicos de la plataforma, elimine la referencia al proyecto principal y haga clic en Aceptar. Luego agrega la referencia nuevamente. (Esto tiene que hacerse porque los proyectos específicos de la plataforma no reconocerán el proyecto central renombrado) </li>
 <li>
 	Cambie la acción de compilación de los siguientes archivos de 'Página' a 'Recurso integrado'(Embedded Resource):
 	<code>
    * Core|Pages|MainPage.xaml
    * Core|App.xaml
 	</code>
 </li>
 </ol>

 <p>Seguiendo los pasos anteriores, MvvmCross ya estaría configurado. Al compilar se debe ver algo similar a la siguiente captura</p>

<img src="/Img/img3_1.png"/>


<h3>Paso 3 - Archivo de arranque de la aplicación. CoreApp</h3>


<code>
 public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            //No es necesario registrar las instancias que termina como Service, se registra automaticamente
            //Mvx.LazyConstructAndRegisterSingleton<IWebClientService, WebClientService>();

            //Configuramos la página de arranque, en nuestro caso será MainViewModel
            RegisterNavigationServiceAppStart<ViewModels.MainViewModel>();
        }

</code>

No es necesario aplicar ningún cambio. Si tenemos un servicio que el nombre no termine por Service, tenemos que registrar de la siguiente forma:

<code>
Mvx.LazyConstructAndRegisterSingleton<INombre, Nombre>();
</code>

<h3>Paso 4 - Crear un archivo de configuración </h3>

<p>Creamos una carpeta con el nombre de Constants y dentro de ella, un CLASS estatica ConfigConstants.cs</p>

<code>
using System;
namespace BaseForms.Constants
{
    public static class ConfigConstants
    {
        public const string keyApi = ""; // Código de la api themoviedb

        public const string baseUrl = "https://api.themoviedb.org/3/movie/upcoming?api_key="+keyApi+"&language=es-ES&page=1";

        public const string imgSmall = "http://image.tmdb.org/t/p/w185";

        public const string imgBig = "http://image.tmdb.org/t/p/w780";
    }
}
</code>


.. en proceso




