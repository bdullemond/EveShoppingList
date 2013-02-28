using System;
using System.Windows;


namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsView : Window
    {
        private SettingsViewModel viewModel;

        public SettingsView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var settingsTuple = (Tuple<int, int>) this.DataContext;
            if (settingsTuple != null)
            {
                this.viewModel = new SettingsViewModel(settingsTuple.Item1, settingsTuple.Item2);
                this.root.DataContext = this.viewModel;
            }
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Tuple<int, int>(this.viewModel.AmmoAmount, this.viewModel.CapChargesAmount);
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
    
}
