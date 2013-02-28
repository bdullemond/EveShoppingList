using System.Collections.Generic;

namespace ShoppingList
{
    public class ItemViewModel : ViewModelBase
    {
        public Item Item { get; set; }

        public List<string> Categories { get; set; }

        public List<string> Items { get; set; }

        public int Quantity { get; set; }

        public ItemViewModel(Item item)
        {
            this.Item = item;
            this.Quantity = item.Quantity;
            var categories = ItemDataSource.GetCategories();
            categories.Sort();
            this.Categories = categories;
            var items =ItemDataSource.GetItems(item.Category);
            items.Sort();
            this.Items = items;
        }

        public void ChangeCategory(string category)
        {
            this.Items = ItemDataSource.GetItems(category);
            OnPropertyChanged("Items");
        }

        public void Save(string category, string item)
        {
            this.Item.Category = category;
            this.Item.Name = item;
            this.Item.Quantity = this.Quantity;
        }

    }
}
