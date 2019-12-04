using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace YTLauncher
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        public MainPage()
        {
            this.InitializeComponent();

            MainPageFrame.Navigated += MainPageFrame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            DisplayInformation.GetForCurrentView().OrientationChanged += OnOrientationChanged;

            #region Titlebar and StatusBar Customization
            //PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {

                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.BackgroundColor = Colors.Red;
                    titleBar.ForegroundColor = Colors.White;
                    titleBar.ButtonBackgroundColor = Colors.Red;
                    titleBar.ButtonForegroundColor = Colors.White;

                    titleBar.InactiveBackgroundColor = Colors.Red;
                    titleBar.InactiveForegroundColor = Colors.White;
                    titleBar.ButtonInactiveBackgroundColor = Colors.Red;
                    titleBar.ButtonInactiveForegroundColor = Colors.White;
                }
            }

            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Red;
                    statusBar.ForegroundColor = Colors.White;
                }
            }
            #endregion
        }

        #region StatusBar show or hide
        private async void OnOrientationChanged(DisplayInformation sender, object args)
        {
            DisplayOrientations displayInfo = DisplayInformation.GetForCurrentView().CurrentOrientation;

            if (ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1, 0))
            {
                var statusBar = StatusBar.GetForCurrentView();

                if (displayInfo.ToString() == "Landscape" || displayInfo.ToString() == "LandscapeFlipped")
                {
                    await statusBar.HideAsync();
                }
                else
                {
                    await statusBar.ShowAsync();
                }
            }
        }
        #endregion       

        #region Navigation Events Handler & App's Title
        private void MainPageFrame_Navigated(object sender, NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            ((Frame)sender).CanGoBack ?
            AppViewBackButtonVisibility.Visible :
            AppViewBackButtonVisibility.Collapsed;
        }        

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            MainPageFrame.Navigate(typeof(Pages.HomePage));
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            if (MainPageFrame.CanGoBack) MainPageFrame.GoBack();
            else e.Handled = false;
        }
        #endregion
    }
}
