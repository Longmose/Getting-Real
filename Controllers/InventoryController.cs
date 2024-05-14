namespace FullyShipd {
	public static class InventoryController {
		private static List<InventoryItem> items;

		static InventoryController() {
			// TODO: Handle if LoadInventory() returns null
			items = DataController.LoadInventory() ?? new List<InventoryItem>();
		}

		// Get all inventory items
		public static List<InventoryItem> GetInventoryItems() {
			return items;
		}

		public static InventoryItem? GetInventoryItem(string sku) {
			return items.FirstOrDefault(item => item.Sku == sku);
		}
	}
}