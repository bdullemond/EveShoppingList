using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShoppingListView : Window
    {
        private readonly ShoppingListViewModel viewModel = new ShoppingListViewModel();

        private ItemView itemView;
        private SettingsView settingsView;
        private Item addingItem;

        public ShoppingListView()
        {
            InitializeComponent();
            using (var stream = new FileStream("ninveah.ico", FileMode.Open))
            {
                this.Icon = BitmapFrame.Create(stream);
            }
            
            this.root.DataContext = this.viewModel;

            this.Closing += OnClose;
        }

        private void OnClose(object sender, CancelEventArgs e)
        {
            if (this.itemView != null)
                this.itemView.Close();
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
            var location = this.SettingsButton.PointToScreen(new Point(0, 0));
            var settingsTuple = new Tuple<int, int>(this.viewModel.DefaultAmmoAmount, this.viewModel.DefaultCapBoosterAmount);
            var view = new SettingsView()
                {
                    ShowActivated = false,
                    DataContext = settingsTuple,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation =WindowStartupLocation.Manual, Top = location.Y, Left = location.X
                };
            view.Show();
            this.settingsView = view;
            this.settingsView.ApplyButton.Click += SettingsViewApplyButton_Click;
        }

        private void SettingsViewApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsTuple = (Tuple<int, int>) this.settingsView.DataContext;
            if (settingsTuple != null)
            {
                this.viewModel.DefaultAmmoAmount = settingsTuple.Item1;
                this.viewModel.DefaultCapBoosterAmount = settingsTuple.Item2;
            }
        }

        private void EditItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            var item = (Item)this.ItemList.SelectedItem;
            if (item != null)
            {
                this.OpenItemView(item);
            }

        }

        private void OpenItemView(Item item)
        {
            var location = this.EditItemButton.PointToScreen(new Point(0, 0));
            var view = new ItemView
                {
                    ShowActivated = false,
                    DataContext = item,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.Manual, Top = location.Y, Left = location.X
                };
            view.Show();
            this.itemView = view;

            this.itemView.Closed += OnItemView_Closed;
        }

        private void OnItemView_Closed(object sender, EventArgs e)
        {
            if (this.addingItem != null)
                this.viewModel.AddItem(this.addingItem);

            this.viewModel.UpdateLists();

            this.addingItem = null;
        }

        private void AddItemButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.addingItem = new Item();
            this.OpenItemView(this.addingItem);
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
