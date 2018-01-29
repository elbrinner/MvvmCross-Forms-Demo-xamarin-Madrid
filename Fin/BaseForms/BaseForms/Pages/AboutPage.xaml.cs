using System;
using System.Collections.Generic;
using BaseForms.Core.ViewModels;
using MvvmCross.Forms.Views;
using MvvmCross.Forms.Views.Attributes;
using Xamarin.Forms;

namespace BaseForms.Core.Pages
{

    //https://www.mvvmcross.com/mvvmcross-53-release/ otros presenter
    [MvxModalPresentation(WrapInNavigationPage = true, Title = "Modal")]
    public partial class AboutPage : MvxContentPage<AboutViewModel>
    {
        public AboutPage()
        {
            InitializeComponent();
        }


    }
}
