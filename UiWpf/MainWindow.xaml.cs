using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UiWpf.ViewModel;

namespace UiWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel.DataModel context;
        public MainWindow()
        {
            InitializeComponent();
            this.context = new DataModel();
            context.selectedGame = "Skyrim";
            this.DataContext = this.context;

        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    usc = new UserControlHome();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemSettings":
                    usc = new UserControlSettings();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemHelp":
                    usc = new UserControlHelp();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }


        private void btnGameSkyrim_Click(object sender, RoutedEventArgs e)
        {
            this.context.selectedGame = "Skyrim";
        }

        private void btnGameSkyrimSE_Click(object sender, RoutedEventArgs e)
        {
            this.context.selectedGame = "SkyrimSE";
        }
    }
}
