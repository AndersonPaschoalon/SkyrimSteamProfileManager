using System.Windows;
using System.Windows.Input;

namespace TreeViewWithCheckBoxes
{
    public partial class Window1 : Window
    {
        FooViewModel rootData;

        public Window1()
        {
            InitializeComponent();

            FooViewModel root = this.tree.Items[0] as FooViewModel;
            this.rootData = root;

            // Undo
            base.CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Undo,
                    (sender, e) => // Execute
                    {                        
                        e.Handled = true;
                        root.IsChecked = false;
                        this.tree.Focus();
                    },
                    (sender, e) => // CanExecute
                    {
                        e.Handled = true;
                        e.CanExecute = (root.IsChecked != false);
                    }));

            this.tree.Focus();
        }

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            FrameworkElement fe = sender as FrameworkElement;
            rootData.ShowChecked();
            //((FooViewModel)fe.DataContext).ShowChecked();
        }
    }
}