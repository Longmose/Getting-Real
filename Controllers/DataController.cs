using Newtonsoft.Json;

namespace FullyShipd {
	public static class DataController {
		private static string inventoryFilePath = "inventory.json";
		private static string ordersFilePath = "orders.json";
		private static string supplierOrdersFilePath = "supplierOrders.json";

		// Tries to load a file and deserialize it with the specified type, returning null if an error occurs
		private static List<T>? TryLoadFile<T>(string filePath) {
			try {
				if(!File.Exists(filePath)) {
					// Create the file if it doesn't exist, with empty content
					File.WriteAllText(filePath, "[]");
					return new List<T>();
				} else {
					// Read the file and deserialize it
					List<T>? items = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(filePath));
					if(items == null) throw new Exception("An error occurred while deserializing the file.");

					return items;
				}
			} catch(Exception ex) {
				Console.WriteLine($"An error occurred while reading from the file: {ex.Message}");
				return null;
			}
		}

		// Tries to save a file with the specified content, returning false if an error occurs
		public static bool TrySaveFile<T>(string filePath, T content) {
			try {
				// Serialize the items and write them to the file
				File.WriteAllText(filePath, JsonConvert.SerializeObject(content, Formatting.Indented));
				return true;
			} catch(Exception ex) {
				Console.WriteLine($"An error occurred while writing to the file: {ex.Message}");
				return false;
			}
		}

		public static List<InventoryItem>? LoadInventory() {
			return TryLoadFile<InventoryItem>(inventoryFilePath);
		}

		public static List<Order>? LoadOrders() {
			return TryLoadFile<Order>(ordersFilePath);
		}

		public static bool UpdateOrder(Order order) {
			var orders = LoadOrders();
			if(orders == null) return false;

			// Make sure the order exists
			var foundOrder = orders.Find(o => o.Id == order.Id);
			if(foundOrder == null) return false;

			// Update the order
			orders[orders.IndexOf(foundOrder)] = order;

			return TrySaveFile<List<Order>>(ordersFilePath, orders);
		}

		public static List<SupplierOrder>? LoadSupplierOrders() {
			return TryLoadFile<SupplierOrder>(supplierOrdersFilePath);
		}

		public static bool SaveSupplierOrders(List<SupplierOrder> orders) {
			return TrySaveFile<List<SupplierOrder>>(supplierOrdersFilePath, orders);
		}

		public static bool AddSupplierOrder(SupplierOrder newOrder) {
			var orders = LoadSupplierOrders();
			if(orders == null) return false;

			orders.Add(newOrder);
			return SaveSupplierOrders(orders);
		}
	}
}
