namespace FullyShipd {
	public class OrderItem {
		public string Sku { get; set; }
		public int Quantity { get; set; }

		public OrderItem(string sku, int quantity) {
			Sku = sku;
			Quantity = quantity;
		}
	}
}
