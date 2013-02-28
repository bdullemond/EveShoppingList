using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ShoppingList
{
    public static class ItemDataSource
    {
        private static readonly List<string> droneCategories = new List<string>
                                                                    {
                                                                        "Combat Drones", 
                                                                        "Mining Drones", 
                                                                        "Combat Utility Drones", 
                                                                        "Logistic Drones", 
                                                                        "Electronic Warfare Drones", 
                                                                        "Salvage Drones"
                                                                    };

        private const string capBoosterChargeCategory = "Cap Booster Charges";

        private const string scriptsCategory = "Scripts";

        private static Dictionary<string, string> items;

        public static Dictionary<string, string> GetItems()
        {
            if (items == null)
            {   
                var result = new Dictionary<string, string>();

                var settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                settings.IgnoreComments = true;
            
                // Create an XmlReader
                using (var reader = XmlReader.Create("data\\items.xml", settings))
                {
                    reader.MoveToContent(); // Skip over the XML declaration

                    while (reader.Read())
                    {
                        if (reader.Name != "Item") 
                            continue;

                        reader.MoveToAttribute("GroupName");
                        var groupName = reader.ReadContentAsString();

                        reader.MoveToAttribute("TypeName");
                        var itemName = reader.ReadContentAsString();

                        result.Add(itemName, groupName);
                    }
                   
                    items = result;
                }
            }
            return items;
        }

        public static List<string> GetDrones()
        {
            var result =  (from kvp in GetItems() where droneCategories.Contains(kvp.Value) select kvp.Key).ToList();
            return result;
        }

        public static List<string> GetCapBoosterCharges()
        {
            var result = (from kvp in GetItems() where kvp.Value == capBoosterChargeCategory select kvp.Key).ToList();
            return result;
        }

        public static List<string> GetCategories()
        {
            var result = GetItems().Values.Distinct().ToList();
            return result;
        }

        public static List<string> GetItems(string category)
        {
            var result = (from kvp in GetItems() where kvp.Value == category select kvp.Key).ToList();
            return result;
        }

        public static List<string> GetScripts()
        {
            var result = (from kvp in GetItems() where kvp.Value == scriptsCategory select kvp.Key).ToList();
            return result;
        }
    }
}
