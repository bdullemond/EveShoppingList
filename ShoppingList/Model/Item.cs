using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Item(string name, int quantity)
        {
            this.Name = name.Trim();
            this.Quantity = quantity;
            this.Category = ItemDataSource.GetItems()[name.Trim()];;
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
