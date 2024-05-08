using System.Reflection.Emit;

namespace FullyShipd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* **Test of Inventory Class** */

            // Create an inventory
            Inventory inventory = new Inventory();

            // Add items to the inventory
            inventory.AddItem(new InventoryItem("SKU-0001", "01-01-01", 10, false));
            inventory.AddItem(new InventoryItem("SKU-0002", "01-01-02", 5, true));

            // Update stock of an existing item
            inventory.UpdateStock("SKU-0001", 15);

            // Check if enough stock exists for an order
            if (inventory.CheckItemStock("SKU-0002", 3))
            {
                Console.WriteLine("Sufficient stock for SKU-0002. Order can be placed.");
            }
            else
            {
                Console.WriteLine("Not enough stock for SKU-0002.");
            }

            // Get all items in the inventory
            List<InventoryItem> allItems = inventory.GetAllItems();
            Console.WriteLine("\nAll items in the inventory:");
            foreach (InventoryItem item in allItems)
            {
                Console.WriteLine(item.ToString());
            }

            // Save inventory to JSON file
            inventory.SaveInvToJson("inventory.json");
            Console.WriteLine("Inventory saved to inventory.json");

            // Load inventory from JSON file
            Inventory loadedInventory = new Inventory();
            loadedInventory.LoadInvFromJson("inventory.json");
            Console.WriteLine("Inventory loaded from inventory.json");

            // Print loaded inventory
            List<InventoryItem> loadedItems = loadedInventory.GetAllItems();
            Console.WriteLine("\nLoaded inventory contents:");
            foreach (InventoryItem item in loadedItems)
            {
                Console.WriteLine(item.ToString());
            }

            Console.ReadLine(); // Keep the console open to view results

            /* **End of Test of Inventory Class** */
        }
    }
}
