using System.Collections.Generic;
using System.Xml;

namespace ShoppingList
{
    public static class ItemDataReader
    {
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

                        reader.MoveToAttribute("groupName");
                        var groupName = reader.ReadContentAsString();

                        reader.MoveToAttribute("typeName");
                        var itemName = reader.ReadContentAsString();

                        result.Add(itemName, groupName);
                    }
                   
                    items = result;
                }
            }
            return items;
        }
    }
}
