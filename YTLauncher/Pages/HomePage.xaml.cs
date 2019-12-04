using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=234238

namespace YTLauncher.Pages
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        ResourceLoader loader;
        List<UriInfo> URIs = new List<UriInfo>();
        public HomePage()
        {
            this.InitializeComponent();
            loader = new ResourceLoader();
            URIs.Add(new UriInfo
            {
                Name = loader.GetString("vndYoutubeName"),
                Uri = "vnd.youtube:{0}",
                Info = loader.GetString("vndYoutubeInfo")
            });
            URIs.Add(new UriInfo
            {
                Name = loader.GetString("awesometubeName"),
                Uri = "atube:watch?v={0}",
                Info = loader.GetString("awesometubeInfo")
            });
            URIs.Add(new UriInfo
            {
                Name = loader.GetString("tubecastName"),
                Uri = "tubecast:VideoPage?VideoID={0}",
                Info = loader.GetString("tubecastInfo")
            });
            URIs.Add(new UriInfo
            {
                Name = loader.GetString("metrotubeName"),
                Uri = "metrotube:VideoPage?VideoID={0}",
                Info = loader.GetString("metrotubeInfo")
            });
            UriComboBox.ItemsSource = URIs;
            var settings = ApplicationData.Current.LocalSettings;
            string protocolUri = "";
            try
            {
                protocolUri = settings.Values["protocolUri"].ToString();
            }
            catch
            {
                protocolUri = "vnd.youtube:{0}";
                settings.Values["protocolUri"] = protocolUri;
            }
            int index = -1;
            for (int i = 0; index == -1 && i < URIs.Count; i++)
            {
                if (protocolUri == URIs[i].Uri) index = i;
            }
            if (index != -1)
            {
                UriComboBox.SelectedIndex = index;
                UriInfoTextBlock.Text = URIs[index].Info;
            }
            else
            {
                UriComboBox.SelectedIndex = 0;
                CustomCheckBox.IsChecked = true;
                CustomUriTextBox.Text = settings.Values["protocolUri"].ToString();
            }
            UriComboBox.SelectionChanged += UriComboBox_SelectionChanged;
        }

        private void UriComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = (ComboBox)sender;
            var item = (UriInfo)combo.SelectedItem;
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["protocolUri"] = item.Uri;
            UriInfoTextBlock.Text = item.Info;
        }

        private void CustomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UriComboBox.IsEnabled = false;
            UriComboBox.SelectedIndex = 0;
            CustomUriTextBox.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UriComboBox.IsEnabled = true;
            CustomUriTextBox.Visibility = Visibility.Collapsed;
            var settings = ApplicationData.Current.LocalSettings;
            var item = (UriInfo)UriComboBox.SelectedItem;
            settings.Values["protocolUri"] = item.Uri;
        }

        private void CustomUriTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values["protocolUri"] = CustomUriTextBox.Text;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Pages.Info));
        }

        private async void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-settings:appsforwebsites"));
        }
    }

    public class UriInfo
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string Info { get; set; }
    }
}
