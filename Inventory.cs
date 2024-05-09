using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FullyShipd
{
    // Class for Items in the inventory
    public class InventoryItem
    {
        public string SKU {  get; set; }
        public string Location { get; set; }
        public int Stock { get; set; }
        public bool IsDropshipped { get; set; }

        public InventoryItem(string sku, string location, int stock, bool isDropshipped)
        {
            SKU = sku;
            Location = location;
            Stock = stock;
            IsDropshipped = isDropshipped;
        }

        public string ToString()
        {
            return $"SKU: {SKU}, Location: {Location}, Stock: {Stock}, IsDropshipped: {IsDropshipped}";
        }
    }

    // Class for Inventory itself
    public class Inventory
    {
        private List<InventoryItem> items;

        public Inventory() 
        { 
            items = new List<InventoryItem>();
        }

        // Add item to Inventory
        public void AddItem(InventoryItem item)
        {
            items.Add(item);
        }

        // Remove item from Inventory
        public void RemoveItem(string sku)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].SKU == sku)
                { 
                    items.RemoveAt(i);
                }
            }
        }

        // Update stock of product
        public void UpdateStock (string sku, int newStock)
        {
            for(int i = 0; i < items.Count; i++) 
            { 
                if (items[i].SKU == sku)
                {
                    items[i].Stock = newStock;
                }
            }
        }

        // Search for item in an order to check if there is enough stock
        public bool CheckItemStock (string sku, int requiredStock)
        {
            for(int i = 0; i < items.Count;i++)
            {
                if (items[i].SKU == sku && items[i].Stock >= requiredStock)
                {
                    return true;
                }
            }
            return false;
        }

        // Get the whole inventory
        public List<InventoryItem> GetAllItems() 
        {
            return items; 
        }

        // Read .json
        public void LoadInvFromJson(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                items = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonString);
            }
            else
            {
                items = new List<InventoryItem>();
                Console.WriteLine("File could not be found. Empty file has been created.");
            }
        }

        // Save to .json
        public void SaveInvToJson(string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(items, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }
    }
}
