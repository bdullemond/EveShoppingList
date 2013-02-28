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
            var drones = ItemDataSource.GetDrones();
            var capBoosterCharges = ItemDataSource.GetCapBoosterCharges();

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
                    this.Items.Add(new Item(tuples[0], 1));
                    this.Items.Add(capBoosterCharges.Contains(tuples[1])
                        ? new Item(tuples[1], capBoosterChargeAmount)
                        : new Item(tuples[1], ammoChargeAmount));
                }
                else
                {
                    var itemName = line;
                    var quantity = 1;
                    if (drones.Any(drone => line.Contains(drone)))
                    {
                        tuples = line.Split('x');
                        itemName = tuples[0];
                        quantity = int.Parse(tuples[1]);
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
