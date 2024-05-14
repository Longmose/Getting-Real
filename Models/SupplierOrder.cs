namespace FullyShipd {
	public class SupplierOrder {
		public string Id { get; set; }
		public DateTime Date { get; set; }
		public int Status { get; set; }
		public List<string> OrderIds { get; set; }
		public List<OrderItem> Items { get; set; }
		public TimeSpan ExpectedDeliveryTime { get; set; }

		public SupplierOrder(string id, DateTime date, int status, List<string> orderIds, List<OrderItem> items, TimeSpan expectedDeliveryTime) {
			Id = id;
			Date = date;
			Status = status;
			OrderIds = orderIds;
			Items = items;
			ExpectedDeliveryTime = expectedDeliveryTime;
		}
	}
}
