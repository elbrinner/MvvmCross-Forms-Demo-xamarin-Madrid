using System;
using System.Collections.Generic;
using MvvmCross.Forms.Views;
using BaseForms.Core.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace BaseForms.Core.Pages
{
    public partial class MainPage : MvxContentPage<MainViewModel>
    {
        public MainPage()
        {

            InitializeComponent();


            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
