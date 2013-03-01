using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Model
{
    public class Settings
    {
        public Settings()
        {
            this.DefaultAmmoAmount = 200;
            this.DefaultCapChargesAmount = 20;
        }

        public int DefaultAmmoAmount { get; set; }

        public int DefaultCapChargesAmount { get; set; }
    }
}
