using System;
using System.Windows;
using ShoppingList.Model;


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
            var settings = (Settings) this.DataContext;
            if (settings != null)
            {
                this.viewModel = new SettingsViewModel(settings.DefaultAmmoAmount, settings.DefaultCapChargesAmount);
                this.root.DataContext = this.viewModel;
            }
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.DataContext = new Settings()
            {
                DefaultAmmoAmount = this.viewModel.AmmoAmount,
                DefaultCapChargesAmount = this.viewModel.CapChargesAmount
            };
            Close();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
    
}
