using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList
{
    [Serializable]
    public class ShipFitting
    {
        readonly List<string> empties = new List<string>() { "[empty high slot]", "[empty med slot]", "[empty low slot]", "[empty rig slot]", };

        public string Fitting { get; set; }

        public List<Item> Items { get; set; }

        public string Name { get; set; }

        public Guid Id { get; set; }

        public ShipFitting()
        {
            this.Items = new List<Item>();
            this.Id = Guid.NewGuid();
        }

        public ShipFitting(string fitting, int ammoChargeAmount, int capBoosterChargeAmount) : this()
        {
            this.Fitting = fitting;

            var lines = fitting.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();
            var firstLine = lines.First();
            
            this.Name = firstLine;
            var shipType = (firstLine.Trim(new char[] { '[', ']' }).Split(','))[0];
            this.Items.Add(new Item(shipType, 1));

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (empties.Contains(line.ToLower()))
                    continue;

                var tuples = line.Split(',');
                if (tuples.Count() > 1)
                {
                    var module = tuples[0];
                    var charge = tuples[1].Trim();

                    this.Items.Add(new Item(module, 1));


                    if (ItemDataSource.GetCapBoosterCharges().Contains(charge, StringComparer.CurrentCultureIgnoreCase))
                    {
                        this.Items.Add(new Item(charge, capBoosterChargeAmount));
                    }
                    else if (ItemDataSource.GetScripts().Contains(charge, StringComparer.CurrentCultureIgnoreCase))
                    {
                        this.Items.Add(new Item(charge, 1));
                    }
                    else
                    {
                        this.Items.Add(new Item(charge, ammoChargeAmount));
                    }
                    
                }
                else
                {
                    var itemName = line;
                    var quantity = 1;
                    if (ItemDataSource.GetDrones().Any(drone => line.ToLower().Contains(drone.ToLower())))
                    {
                        tuples = line.Split('x');
                        itemName = tuples[0];
                        quantity = tuples.Length > 1 ? int.Parse(tuples[1]) : 1;

                    }

                    this.Items.Add(new Item(itemName, quantity));
                }
                
            }
        }

        public ShipFitting Clone()
        {
            var newFitting = new ShipFitting
                {
                    Id = Guid.NewGuid(), 
                    Name = this.Name, 
                    Fitting = this.Fitting, 
                    Items = this.Items
                };
            return newFitting;
        }
        
    }
}
