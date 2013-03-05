using System;


namespace ShoppingList
{
    [Serializable]
    public class Item
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int Quantity { get; set; }

        public Item()
        {
            this.Quantity = 0;
        }

        public Item(Item item)
        {
            this.Name = item.Name;
            this.Category = item.Category;
            this.Quantity = item.Quantity;
        }

        private const string errorTemplate = "The item: {0} was not found in the item database.";
        
        public Item(string name, int quantity)
        {
            this.Name = ItemDataSource.GetConsistentItemName(name.Trim());
            if (this.Name == null)
                throw new ArgumentOutOfRangeException("name", string.Format(errorTemplate, name));

            this.Quantity = quantity;
            this.Category = ItemDataSource.GetItems()[this.Name];
        }

        public void Add(int quantity)
        {
            this.Quantity += quantity;
        }

        public bool Subtract(int quantity)
        {
            this.Quantity -= quantity;
            return this.Quantity <= 0;
        }

     

    }
}
