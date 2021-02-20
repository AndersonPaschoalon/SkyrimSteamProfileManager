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
using ProfileManager;
using ProfileManager.Objects;
namespace UiWpf
{
    /// <summary>
    /// Interaction logic for UserControlHome.xaml
    /// </summary>
    public partial class UserControlHome : UserControl
    {
        private ViewModel.DataModel context;
        public UserControlHome()
        {
            InitializeComponent();
            this.context = new DataModel();
            this.sample_entries();
            this.DataContext = this.context;
        }

        private void sample_entries()
        {
            SPProfile profAct = new SPProfile();
            SPProfile profD1 = new SPProfile();
            SPProfile profD2 = new SPProfile();
            SPProfile profD3 = new SPProfile();
            profAct.name = "Oldrim Vanilla";
            profD1.name = "Skyrim Vanilla";
            profD2.name = "Skyrim Dev";
            profD3.name = "Skyrim Modded";
            List<SPProfile> listD = new List<SPProfile>();
            listD.Add(profD1);
            listD.Add(profD2);
            listD.Add(profD3);
            this.context.desactivatedProf = listD;
            this.context.activeProf = profAct;

        }

        private void btnDesactivate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Switch_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnActivate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
