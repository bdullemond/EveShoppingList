using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList
{
    public class SettingsViewModel : ViewModelBase
    {
        public int AmmoAmount { get; set; }

        public int CapChargesAmount { get; set; }

        public SettingsViewModel(int ammo, int capCharges)
        {
            this.AmmoAmount = ammo;
            this.CapChargesAmount = capCharges;

        }
    }
}
