using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace GitSpear
{
    public class FooViewModel : INotifyPropertyChanged
    {
        #region Data

        bool? _isChecked = false;
        FooViewModel _parent;

        #endregion // Data

        #region CreateFoos


        public static FooViewModel CreateFooRec(string dirName)
        {
            // directory info
            DirectoryInfo di = new DirectoryInfo(dirName);
            // dir model
            FooViewModel viewModel = new FooViewModel(dirName);
            viewModel.Children = new List<FooViewModel>();

            // not base case: return a view model with no children
            if (di.GetDirectories().Length != 0)
            {
                DirectoryInfo[] listDirs = di.GetDirectories();
                int countDirs = listDirs.Length;
                for (int i = 0; i < countDirs; i++)
                {
                    try
                    {
                        var dir = listDirs[i];
                        FooViewModel child = CreateFooRec(dir.FullName);
                        child.IsInitiallySelected = false;
                        viewModel.Children.Add(child);
                    }
                    catch (UnauthorizedAccessException ex1)
                    {
                        Console.WriteLine("Access to directory Unauthorized. Message:" + ex1.Message + ", StackTrace:" + ex1.StackTrace);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("** UNESPECGED EXCEPTION. Message:" + ex2.Message + ", StackTrace:" + ex2.StackTrace);
                    }
                }
            }
            // base case
            viewModel.IsInitiallySelected = false;
            return viewModel;
        }

        public static List<FooViewModel> BuildFooViewModel(string rootDir)
        {
            FooViewModel root = CreateFooRec(rootDir);
            root.Initialize();
            return new List<FooViewModel> { root };
        }

        public static List<FooViewModel> CreateFoos()
        {
            const string dir = @"C:\Users\anderson_paschoalon\Pictures";
            return BuildFooViewModel(dir);
        }
        
        FooViewModel(string dirName)
        {
            this.DirFullName = dirName;
            dirName.TrimEnd('\\');
            string[] listDirs = dirName.Split('\\');
            string lastDir = listDirs[listDirs.Length - 1];
            this.Name = lastDir;
            this.Children = new List<FooViewModel>();
        }

        void Initialize()
        {
            foreach (FooViewModel child in this.Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

#endregion // CreateFoos

#region Properties

        public List<FooViewModel> Children { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; private set; }

        public string DirFullName { get; private set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { this.SetIsChecked(value, true, true); }
        }

        public void ShowChecked()
        {
            List<string> dirs = ShowCheckedRec(this);
            string csvDirs = "";
            foreach (var item in dirs)
            {
                csvDirs += item + ", ";
            }
            MessageBox.Show(csvDirs, "Debug", MessageBoxButton.OK);
        }

        public List<string> ShowCheckedRec(FooViewModel model)
        {
            List<string> checkedDirs = new List<string>();
            // base case: is checked, all dirs belows are checked implictly
            if (model.IsChecked == true)
            {
                checkedDirs.Add(model.DirFullName);
                return checkedDirs;
            }
            // base case: no child
            if (model.Children.Count == 0)
            {
                return checkedDirs;
            }
            // general case
            foreach (var item in model.Children)
            {
                List<string> itemChecked = ShowCheckedRec(item);
                if (itemChecked.Count > 0)
                {
                    checkedDirs.AddRange(itemChecked);
                }
            }
            return checkedDirs;
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                this.Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            this.OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.Children.Count; ++i)
            {
                bool? current = this.Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsChecked(state, false, true);
        }

#endregion // IsChecked

#endregion // Properties

#region INotifyPropertyChanged Members

        void OnPropertyChanged(string prop)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

#endregion
    }
}