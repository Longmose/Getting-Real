namespace FullyShipd {
	public class InventoryItem {
		public string Sku { get; set; }
		public string Location { get; set; }
		public int Stock { get; set; }
		public bool IsDropshipped { get; set; }

		public InventoryItem(string sku, string location, int stock, bool isDropshipped) {
			Sku = sku;
			Location = location;
			Stock = stock;
			IsDropshipped = isDropshipped;
		}
	}
}
