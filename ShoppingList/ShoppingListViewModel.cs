using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;
using ShoppingList.Annotations;

namespace ShoppingList
{
    public class ShoppingListViewModel : INotifyPropertyChanged
    {
       
        private readonly Dictionary<string, Item> items = new Dictionary<string, Item>();
        private readonly Dictionary<Guid, ShipFitting> shipFittings = new Dictionary<Guid, ShipFitting>(); 

        public ObservableCollection<Item> Items { get; private set; }
        public ObservableCollection<ShipFitting> ShipFittings { get; set; }

        public int DefaultAmmoAmount { get; set; }

        public int DefaultCapBoosterAmount { get; set; }

        private bool isDirty;
        public bool IsDirty
        {
            get { return this.isDirty; }
            set 
            {
                if (value == this.isDirty) return;
                this.isDirty = value;
                OnPropertyChanged();
            }
        }

        public ShoppingListViewModel()
        {
            this.Items = new ObservableCollection<Item>();
            this.ShipFittings = new ObservableCollection<ShipFitting>();
            this.DefaultAmmoAmount = 200;
            this.DefaultCapBoosterAmount = 20;
            this.IsDirty = false;
        }

        #region methods

        public void Add(string fitting)
        {
            var shipFitting = new ShipFitting(fitting, this.DefaultAmmoAmount, this.DefaultCapBoosterAmount);
            this.Add(shipFitting);
        }

        public void Add(ShipFitting shipFitting)
        {
            this.shipFittings.Add(shipFitting.Id, shipFitting);
            this.AddShipFitItems(shipFitting);
        }

        public void AddShipFitItems(ShipFitting shipFitting)
        {
            foreach (var item in shipFitting.Items)
            {
                if (items.ContainsKey(item.Name))
                {
                    var existingItem = this.items[item.Name];
                    existingItem.Add(item.Quantity);
                }
                else
                {
                    // Make a new item to prevent deep references.
                    items.Add(item.Name, new Item(item));
                }
            }
            this.UpdateLists(); 
        }

        public void Remove(ShipFitting shipFitting)
        {
            this.shipFittings.Remove(shipFitting.Id);

            foreach (var item in shipFitting.Items)
            {
                var existingItem = this.items[item.Name];
                var isEmpty = existingItem.Subtract(item.Quantity);
                if (isEmpty)
                    this.items.Remove(item.Name);
            }
            this.UpdateLists(); 
        }

        public void UpdateLists()
        {
            this.Items.Clear();
            foreach (var item in this.items.Values.OrderBy(i => i.Category).ThenBy(i => i.Name))
            {
                this.Items.Add(item);
            }

            this.ShipFittings.Clear();
            foreach (var fit in this.shipFittings.Values.OrderBy(sf => sf.Name))
            {
                this.ShipFittings.Add(fit);
            }
            this.IsDirty = true;
        }


        public void Load(string filePath)
        {

            List<ShipFitting> fits;
            var formatter = new XmlSerializer(typeof(List<ShipFitting>));
            using (var aFile = new FileStream(filePath, FileMode.Open))
            {
                var buffer = new byte[aFile.Length];
                aFile.Read(buffer, 0, (int) aFile.Length);
                using (var stream = new MemoryStream(buffer))
                {
                    fits = (List<ShipFitting>) formatter.Deserialize(stream);
                }

            }
            this.shipFittings.Clear();
            this.items.Clear();
            
            foreach (var shipFitting in fits)
            {
                this.Add(shipFitting);
            }
        }


        public void Save(string filePath)
        {
            using (var outFile = File.Create(filePath))
            {
                var formatter = new XmlSerializer(typeof (List<ShipFitting>));
                formatter.Serialize(outFile, this.ShipFittings.ToList());
            }
            this.IsDirty = false;
        }

        public void Clear()
        {
            this.shipFittings.Clear();
            this.items.Clear();
            this.isDirty = false;
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        private const string clipboardFormat = "{0} - {1} \\ {2}";

        public string GetList()
        {
            var sb = new StringBuilder();

            foreach (var item in this.Items)
            {
                sb.AppendLine(string.Format(clipboardFormat, item.Quantity, item.Category, item.Name));
            }
            return sb.ToString();
        }
    }
}
