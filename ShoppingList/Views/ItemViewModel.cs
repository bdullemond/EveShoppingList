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
            this.Items = this.GetSortedItems(item.Category);
        }

        public void ChangeCategory(string category)
        {
            this.Items = this.GetSortedItems(category);
            OnPropertyChanged("Items");
        }

        private List<string> GetSortedItems(string category)
        {
            var items = ItemDataSource.GetItems(category);
            items.Sort();
            return items;
        }

        public void Save(string category, string item)
        {
            this.Item.Category = category;
            this.Item.Name = item;
            this.Item.Quantity = this.Quantity;
        }

    }
}
