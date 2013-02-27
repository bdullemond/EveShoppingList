using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShoppingListView : Window
    {
        private readonly ShoppingListViewModel viewModel = new ShoppingListViewModel();

        public ShoppingListView()
        {
            InitializeComponent();
            using (var stream = new FileStream("ninveah.ico", FileMode.Open))
            {
                this.Icon = BitmapFrame.Create(stream);
            }
            
            this.root.DataContext = this.viewModel;
        }


        
        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.Add(this.EntryBox.Text);
            this.EntryBox.Clear();
       }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fitting = (ShipFitting)this.ShipFitList.SelectedItem;
            if (fitting != null)
                this.viewModel.Remove(fitting);
            this.EntryBox.Clear();
       }

        private void ShipFitList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var fitting = (ShipFitting)this.ShipFitList.SelectedItem;
            if (fitting != null)
                this.EntryBox.Text = fitting.Fitting;
        }

        private void LoadButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.viewModel.IsDirty)
                if (MessageBox.Show("You have unsaved changes. Continue?", "Unsaved Changes", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            
            // Create an instance of the open file dialog box.
            var openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "Shopping Lists (.sl)|*.sl|All Files|*.*";
            openFileDialog1.FilterIndex = 1;

            openFileDialog1.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = openFileDialog1.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                var file = openFileDialog1.FileName;
                this.viewModel.Load(file);
            }
            
        }

        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            // Create an instance of the open file dialog box.
            var saveFileDialog = new SaveFileDialog();

            // Set filter options and filter index.
            saveFileDialog.Filter = "Shopping Lists (.sl)|*.sl|All Files|*.*";
            saveFileDialog.FilterIndex = 1;

            // Call the ShowDialog method to show the dialog box.
            bool? userClickedOK = saveFileDialog.ShowDialog();

            // Process input if the user clicked OK.
            if (userClickedOK == true)
            {
                var file = saveFileDialog.FileName;
                this.viewModel.Save(file);
            }
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void EditItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AddItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void NewButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.viewModel.Clear();
        }

        private void ClipboardButton_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.viewModel.GetList());
            MessageBox.Show("Copied to clipboard!", "Copy");
        }

        private void AddFittingButton_OnClick(object sender, RoutedEventArgs e)
        {
            var fitting = (ShipFitting)this.ShipFitList.SelectedItem;
            if (fitting != null)
                this.viewModel.Add(fitting.Clone());
        }
    }
}
