using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace ShoppingList
{
    /// <summary>
    /// Interaction logic for ItemView.xaml
    /// </summary>
    public partial class ItemView : Window
    {
        private ItemViewModel viewModel;

        public ItemView()
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
        }

        public void OnDataContextChanged(object o, DependencyPropertyChangedEventArgs eventArgs)
        {
            var item = (Item)this.DataContext;

            if (item != null)
            {
                this.viewModel = new ItemViewModel(item);
                this.root.DataContext = this.viewModel;
            }
        }

        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var category = (string) this.CategoryList.SelectedItem;
            this.viewModel.ChangeCategory(category);
        }

        private void QuantityBox_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var category = (string) this.CategoryList.SelectedItem;
            var item = (string) this.ItemList.SelectedItem;

            if (category == null || item == null)
            {
                throw new Exception("The category and/or item cannot be null");
            }
            this.viewModel.Save(category, item );
            this.Close();
        }
    }
}
