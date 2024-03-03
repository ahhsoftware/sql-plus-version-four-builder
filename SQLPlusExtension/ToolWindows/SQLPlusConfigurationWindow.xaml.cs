using Microsoft.VisualStudio.PlatformUI;
using SQLPlusExtension.Models;
using System.Diagnostics;
using System.Windows.Controls;

namespace SQLPlusExtension
{
    public partial class SQLPlusConfigurationWindow : DialogWindow
    {
        /// <summary>
        /// Used to launch a browser on a hyperlink click
        /// </summary>
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        public SQLPlusConfigurationWindow(SQLPlusConfigurationWindowViewModel dataContext)
        {
            this.DataContext = dataContext;
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
        {
            var scrollView = (ScrollViewer)sender;

            if (e.ExtentHeightChange != 0)
            {
                scrollView.ScrollToVerticalOffset(scrollView.ExtentHeight);
            }
        }
    }
}
