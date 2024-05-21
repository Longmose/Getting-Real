namespace FullyShipd {
	public class Order {
		public string Id { get; set; }
		public DateTime Date { get; set; }
		public int Status { get; set; }
		public string? SupplierOrderId { get; set; }
		public List<OrderItem> Items { get; set; }

		public Order(string id, DateTime date, int status, string? supplierOrderId, List<OrderItem> items) {
			Id = id;
			Date = date;
			Status = status;
			SupplierOrderId = supplierOrderId;
			Items = items;
		}
	}
}
