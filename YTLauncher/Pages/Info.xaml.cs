using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Email;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
    public sealed partial class Info : Page
    {
        ResourceLoader loader;
        public Info()
        {
            this.InitializeComponent();
            loader = new ResourceLoader();
            VersionTextBlock.Text = string.Format("{0}: {1}.{2}.{3}.{4}",
                loader.GetString("version"),
                Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build,
                Package.Current.Id.Version.Revision);
            AuthorTextBlock.Text = string.Format("{0}: {1}",
                loader.GetString("author"),
                Package.Current.PublisherDisplayName);
        }

        private async void FeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            EmailMessage emailMessage = new EmailMessage();
            emailMessage.To.Add(new EmailRecipient("feedback.massijay@outlook.com"));
            emailMessage.Subject = Package.Current.DisplayName + " feedback";
            emailMessage.Body = string.Format("\n\n{5} {0}: {1}.{2}.{3}.{4}",
                loader.GetString("version"),
                Package.Current.Id.Version.Major,
                Package.Current.Id.Version.Minor,
                Package.Current.Id.Version.Build,
                Package.Current.Id.Version.Revision,
                Package.Current.DisplayName);
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private async void VisitButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://www.windowsblogitalia.com/"));
        }
    }
}
