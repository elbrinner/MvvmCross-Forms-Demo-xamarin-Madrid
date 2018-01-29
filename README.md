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

<img src="/Img/3_1.png"/>


<h3>Paso 3 - Archivo de arranque de la aplicación. CoreApp</h3>

<pre>
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
</pre>

No es necesario aplicar ningún cambio. Si tenemos un servicio que el nombre no termine por Service, tenemos que registrar de la siguiente forma:

<code>
Mvx.LazyConstructAndRegisterSingleton<INombre, Nombre>();
</code>

<h3>Paso 4 - Crear un archivo de configuración </h3>

<p>Creamos una carpeta con el nombre de Constants y dentro de ella, un CLASS estatica ConfigConstants.cs</p>

<pre>
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
</pre>

<h3>Paso 5 - Crear las interfaces de los servicios</h3>


<p>En el proyecto de la PCL, vamos a crear una carpeta Interfaces y dentro de la misma 2 carpetas más Iconnectors y IWebServices</p>

<p>Dentro de la carpeta Iconnectors, crearemos la interfaz para acceder a HttpClient</p>

<h4>IWebClientService.cs</h4>

<pre>
<code>
using System;
using System.Net.Http;

namespace BaseForms.Interfaces.IConnectors
{
    public interface IWebClientService
    {
        HttpClient Client();   
    }
}

</code>
</pre>

<p>* Es ejemplo es muy básico, haga la interfaz según tu necesidad</p>


<p> La implementación de este archivo sería</p>

<pre>

<code>

using System;
using System.Net.Http;
using BaseForms.Interfaces.IConnectors;
using ModernHttpClient;

namespace BaseForms.Service.Connectors
{
    public class WebClientService : IDisposable, IWebClientService
    {
        private HttpClient client;

        public NativeCookieHandler CookieContainer { get; set; }

        public WebClientService()
        {
            this.client = this.GenerateHttpClient();
        }

        private HttpClient GenerateHttpClient()
        {
            this.CookieContainer = new NativeCookieHandler();

            NativeMessageHandler httpClientHandler = new NativeMessageHandler(
                false,
                true,
                this.CookieContainer
            );

            return new HttpClient(httpClientHandler);

        }

        public HttpClient Client()
        {
            return this.client;
        }


        public void Dispose()
        {
            this.client.Dispose();
        }
    }

}	

</code>

</pre>


<p>Para que funcione este código vamos a tener que instalar <strong>ModernHttpClient</strong>.</p>

<img src="/Img/img5.png"/> 

<h4>IMovieWebService</h4>
<p> Agregaremos todos los servicios relacionados con las peliculas aqui</p>

<pre>
<code>
using System;
using System.Threading.Tasks;
using BaseForms.Models.Movie;

namespace BaseForms.Interfaces.IWebServices
{
    public interface IMovieWebService
    {
        Task<MovieResponse> Movie();
    }
}

</code>
</pre>


<pre>
<code>
	
using System;
using System.Globalization;
using System.Threading.Tasks;
using BaseForms.Constants;
using BaseForms.Interfaces.IConnectors;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using Newtonsoft.Json;

namespace BaseForms.Service.WebServices
{
    public class MovieWebService : IMovieWebService
    {
        IWebClientService conection;

        public MovieWebService(IWebClientService conection)
        {
            this.conection = conection;
        }

        public async Task<MovieResponse> Movie()
        {
            var url = new Uri(string.Format(CultureInfo.InvariantCulture, ConfigConstants.baseUrl));
            var result = await conection.Client().GetAsync(url);
            if (result.IsSuccessStatusCode)
            {
                string content = await result.Content.ReadAsStringAsync();
                if (content != null)
                {
                    var response = JsonConvert.DeserializeObject<MovieResponse>(content);
                    response.IsCorrect = true;
                    return response;
                }
            }
            return new MovieResponse() { Mensage = "Servicio no disponible" };
        }
    }
}

</code>
</pre>


<p>Para que funcione y compile, tenemos que instalar <strong>newtonsoft</strong> y crear los modelos.</p>

<img src="/Img/img4.png"/> 


<h3>Paso 7 - Crear los modelos</h3>

<p>Para manejar los datos devuelvo por el servicio, vamos a crear 3 <strong>Modelos</strong>.

<ul>
<li>BaseResponse.cs, para ayudar a tratar los errores</li>
<li>MovieResponse.cs, respuesta del servicio</li>
<li>ResultMovie.cs , es un objeto de MovieResponse, que va a devuelver el detalle por cada pelicula.</li>

</ul>


<h4>BaseResponse.cs</h4>
<p>De forma predetermiada, inicializamos que la petición no ha sido bien realizada. Usaremos la propiedad IsCorrect para conocer si el resultado de la peticón ha sido código 200 o no.</p>
<pre>
<code>
namespace BaseForms.Models.Base
{
    public class BaseResponse
    {
        public string Mensage { get; set; }
        public bool IsCorrect { get; set; }
        public BaseResponse()
        {
            Mensage = "No se ha podido realizar la operación";
            IsCorrect = false;
        }

    }
}	
</code>
</pre>


<h4>MovieResponse.cs</h4>

<p>Respuesta del servicio, trataremos solo algunos campos usando la propiedad <strong>JsonProperty</strong> para cambiar el nombre de la propiedad.</p>
<pre>
<code>
using System;
using System.Collections.Generic;
using BaseForms.Models.Base;
using Newtonsoft.Json;

namespace BaseForms.Models.Movie
{
    public class MovieResponse : BaseResponse
    {

        [JsonProperty("results")]
        public List<ResultMovie> Movies { get; set; }

        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        //public Dates dates { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

    }
}

</code>
</pre>


<h4>ResultMovie.cs</h4>
<p>Modelo que contiene la información a pintar en la pantalla.</p>
<pre>
<code>
using System;
using BaseForms.Constants;
using BaseForms.Converters.Json;
using Newtonsoft.Json;

namespace BaseForms.Models.Movie
{
    public class ResultMovie 
    {
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty("video")]
        public bool Video { get; set; }

        [JsonProperty("vote_average")]
        public double VoteAverage { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("backdrop_path")]
        public string Backdrop { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        public string ImgSmall => string.Format("{0}{1}", ConfigConstants.imgSmall, Backdrop);

        public string ImgBig => string.Format("{0}{1}", ConfigConstants.imgBig, Backdrop);

        /*
         * Propiedades sin tratar
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<object> genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string release_date { get; set; }
        */

    }
}	
</code>
</pre>

<p>Cabe destacar que se puede crear converters para tratar la información retornada por el servicios y que tambien se puede unir y crear nuevas propiedades igual a ImgSmall y ImgBig.</p>


<h3>Paso 8 - Crear el converter JSON/newsoft</h3>

<pre>
<code>
using System;
using Newtonsoft.Json;

namespace BaseForms.Converters.Json
{
    public class BoolConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value.ToString() == "1";
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }
    }
}
</code>
</pre>

<h3>Paso 9 - Crear el ViewModel Base</h3>

<p>Todo lo que es generico en todos los viewmodels, lo meteremos aqui.En nuestro caso sería:</p>

<ul>
<li>La navegación: navigationService</li>
<li>El web services de las peliculas: webMovieService</li>
<li>IsBusy , responsable por indicar si está esperando la respuesta del servicio o no.</li>
<li>Title , responsable por el título de la página</li>
</ul>

<pre>
<code>
using System;
using BaseForms.Interfaces.IWebServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.ViewModels
{
    public class BaseViewModel : MvxViewModel<object>
    {
        protected readonly IMvxNavigationService navigationService;
        protected readonly IMovieWebService webMovieService;
        protected bool isBusy;
        protected string title;
        public BaseViewModel(IMvxNavigationService navigationService,IMovieWebService webMovieService)
        {
            this.navigationService = navigationService;
            this.webMovieService = webMovieService;
        }

        public bool IsBusy
        {
            get
            {
                return this.isBusy;
            }
            set
            {
                this.isBusy = value;
                this.RaisePropertyChanged(() => this.IsBusy);
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                this.RaisePropertyChanged(() => this.Title);
            }
        }



        public override void Prepare(object parameter)
        {

        }
    }
}


</code>
</pre>


<h3>Paso 10 - Crear el ViewModel MainViewModel</h3>

<p>Este viewmodel va a contener:</p>

<ul>
  <li>Herenda de BaseViewModel</li>
  <li>Movies , listado de peliculas</li>
  <li>ResultMovie, pelicula seleccionada</li>
  <li>ViewAppeared, relacionado con el ciclo de vida de la aplicación. Se entra en está función cuando la vista se carga.</li>
  <li>OpenModal , navega a otra página con los valores de la pelicula seleccionada</li>
</ul> 

<pre>
<code>
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using BaseForms.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.Core.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
      
        public MainViewModel(IMvxNavigationService navigationService, IMovieWebService webMovieService) : base(navigationService, webMovieService)
        {
            
        }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public async override void ViewAppeared()
        {

            this.IsBusy = true;
            var response = await this.webMovieService.Movie();
            this.IsBusy = false;
            if (response.IsCorrect)
            {
                Movies = response.Movies;
            }
            else
            {
                //mostrar el mensaje de error
            }

        }

        private List<ResultMovie> movies;
        public List<ResultMovie> Movies
        {
            get
            {
                return this.movies;
            }

            set
            {
                this.movies = value;
                this.RaisePropertyChanged();

            }
        }
        private ResultMovie selectedMovie;
        public ResultMovie SelectedMovie
        {
            get
            {
                return this.selectedMovie;
            }

            set
            {
                this.selectedMovie = value;
                this.RaisePropertyChanged();
                this.OpenModal(value);
            }
        }

        private void OpenModal(ResultMovie value)
        {
            object parameter = value;
            navigationService.Navigate<AboutViewModel, object>(parameter);

        }
    }
}

</code>
</pre>


<h3>Paso 11 - Vista MainView</h3>
<p>Preparando la vista, elementos a tener en cuenta:</p>

<ul>
<li>MvxContentPage, fijate que estamos usando MvxContentPage en lugar de ContentPage</li>
<li>Que estamos relacionando el viewmodel con la vista. <strong>d:MvxContentPage x:TypeArguments="viewModels:MainViewModel</strong></li>
<li>Que estamos cargando el template <strong>xmlns:templates="clr-namespace:BaseForms.Views.Template;assembly=BaseForms"</strong></li>
<li>Que los valores que se pinta, viene de la propiedad <strong>Movies</strong></li>
<li><strong>SelectedMovie</strong> se usa para guardar el elemento seleccionado.</li>
</ul>

<pre>
<code>
<?xml version="1.0" encoding="UTF-8"?>
<d:MvxContentPage x:TypeArguments="viewModels:MainViewModel"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:BaseForms.Core.Pages"
    x:Class="BaseForms.Core.Pages.MainPage"
    xmlns:templates="clr-namespace:BaseForms.Views.Template;assembly=BaseForms"
    xmlns:viewModels="clr-namespace:BaseForms.Core.ViewModels;assembly=BaseForms.Core"
    xmlns:d="clr-namespace:MvvmCross.Forms.Views;assembly=MvvmCross.Forms">
	<ContentPage.Content>
       <ListView SeparatorVisibility="None" 
         ItemsSource="{Binding Movies}" 
            SelectedItem="{Binding SelectedMovie, Mode=TwoWay}">

            <ListView.ItemTemplate>
                    <DataTemplate>
                        <ViewCell>
                            <templates:ItemListImageTemplate />
                        </ViewCell>
                    </DataTemplate>
            
                </ListView.ItemTemplate>
        </ListView>
	</ContentPage.Content>
</d:MvxContentPage>
</code>
</pre>

<h3>Paso 12 - Crear template</h3>

<p>Creamos una carpeta Views en el proyecto BaseForms.Core y luego otro carpeta más con el nombre de <strong>template</strong> dentro de la carpeta <strong>Views</strong>. 

<p>El próximo paso es crear nuestros templates distinto, para esto debemos crear una página <strong>contentView</strong></p>

<h5>Template 1 con imagen</h5>

Código aqui <a href="https://raw.githubusercontent.com/elbrinner/MvvmCross-Forms-Demo-xamarin-Madrid/master/Fin/BaseForms/BaseForms/Views/Template/ItemListImageTemplate.xaml">Template 1</a>
<pre>
<code>
	
	<?xml version="1.0" encoding="UTF-8"?>
<ContentView xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    x:Class="BaseForms.Views.Template.ItemListImageTemplate">
	<ContentView.Content>
       
        <StackLayout Orientation="Horizontal" HorizontalOptions="Fill" Margin="1">
         <Image Source="{Binding ImgSmall}" HorizontalOptions="End"/>
            <StackLayout Orientation="Vertical">
                <Label Text = "{Binding Title}" FontSize="18"/>
                <Label Text = "{Binding OriginalTitle}" FontSize="10"/>  
             </StackLayout>
         </StackLayout>

            
	</ContentView.Content>
</ContentView>

</code>
</pre>

<h5>Template 2 sin imagen</h5>

<pre>
<code>
	
<?xml version="1.0" encoding="UTF-8"?>
<ContentView xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml" x:Class="BaseForms.Views.Template.ItemListTextTemplate">
	<ContentView.Content>
        <StackLayout
            Orientation="Horizontal">
        <StackLayout
              Orientation="Vertical">
            <Label
                Text="{Binding Title}"
                TextColor="#f35e20" />
        </StackLayout>
            
         </StackLayout>
	</ContentView.Content>
</ContentView>

</code>
</pre>


<p>Los templates son buenas formas de reutilizar código, para modificar el aspecto de la lista lo único que debemos indicar ahora es el nombre del template</p>

<pre>
<code>
 <templates:NombreDelTemplateAqui />
</code>

</pre>

<h3>Paso 13 - Crear el ViewModel AboutViewModel</h3>

<p>Este viewmodel contiene:</p>

<ul>
<li>CloseCommand => Un Command para cerrar la modal</li>
<li>La propiedad SelectedMovie, para pintar los datos en la pantalla</li>
<li><strong>Prepare</strong> , este metodo, recupera el valor pasando por el MainViewModel.</li>
</ul>

<pre>
<code>
using System;
using BaseForms.Interfaces.IWebServices;
using BaseForms.Models.Movie;
using BaseForms.ViewModels;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;

namespace BaseForms.Core.ViewModels
{

    public class AboutViewModel : BaseViewModel
    {
       

        private IMvxCommand closeCommand;
        public IMvxCommand CloseCommand => closeCommand = closeCommand ?? new MvxCommand(DoCloseHandler);

        private ResultMovie selectedMovie;

        public AboutViewModel(IMvxNavigationService navigationService, IMovieWebService webMovieService) : base(navigationService, webMovieService)
        {
        }

        public ResultMovie SelectedMovie
        {
            get
            {
                return this.selectedMovie;
            }

            set
            {
                this.selectedMovie = value;
                this.RaisePropertyChanged();
            }
        }

        private void DoCloseHandler()
        {
            Close(this);
        }

        public override void Prepare(object parameter)
        {
            this.SelectedMovie = (ResultMovie)Convert.ChangeType(parameter, typeof(ResultMovie));
            this.Title = this.SelectedMovie.Title;
        }
    }
}

</code>
</pre>


<h3>Paso 14 - Crear la AboutView</h3>

Código aqui <a href="https://raw.githubusercontent.com/elbrinner/MvvmCross-Forms-Demo-xamarin-Madrid/master/Fin/BaseForms/BaseForms/Pages/AboutPage.xaml">Vista AboutView</a>
<pre>
<code>

<?xml version="1.0" encoding="UTF-8"?>
<d:MvxContentPage x:TypeArguments="viewModels:AboutViewModel"
    xmlns="http://xamarin.com/schemas/2014/forms"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:local="clr-namespace:BaseForms.Core.Pages"
    x:Class="BaseForms.Core.Pages.AboutPage"
    xmlns:viewModels="clr-namespace:BaseForms.Core.ViewModels;assembly=BaseForms.Core"
    xmlns:d="clr-namespace:MvvmCross.Forms.Views;assembly=MvvmCross.Forms"
     Title="{Binding Title}">
      <ContentPage.ToolbarItems>
         <ToolbarItem Name="Cerrar" Command="{Binding CloseCommand}"></ToolbarItem>
    </ContentPage.ToolbarItems>
    <ContentPage.Content>
        <StackLayout HorizontalOptions="StartAndExpand">
            
            <Image Aspect="AspectFill" Source="{Binding SelectedMovie.ImgBig}"></Image>           
            <Label Text="{Binding SelectedMovie.Overview}"></Label>
            
        </StackLayout>
    </ContentPage.Content> 
</d:MvxContentPage>

</code>
</pre>


<p> Para que se visualize como modal, debemos agregar el tipo de presentación en la vista. (MvxModalPresentation)</p>


<pre>
<code>
using System;
using System.Collections.Generic;
using BaseForms.Core.ViewModels;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Xamarin.Forms;

namespace BaseForms.Core.Pages
{

    [MvxModalPresentation(WrapInNavigationPage = true, Title = "Modal")]
    public partial class AboutPage : MvxContentPage<AboutViewModel>
    {
        public AboutPage()
        {
            InitializeComponent();
        }


    }
}

</code>
</pre>




<p>El resultado final de este turtorial, se puede ver en la carpeta "FIN" de este repositorio.</p>

<img src="/Img/listado.png"/> 
<img src="/Img/detalle.png"/> 
