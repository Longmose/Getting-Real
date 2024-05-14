namespace FullyShipd {
	public static class SupplierOrderController {
		
		public static string FormatStatus(int status) {
			switch(status) {
				case 0: return "Accepted";
				case 1: return "Ready for pickup";
				case 2: return "Completed";
				default: return "Unknown";
			}
		}

		public static List<SupplierOrder> GetOrders() {
			// TODO: Handle if LoadSupplierOrders() returns null
			return DataController.LoadSupplierOrders() ?? new List<SupplierOrder>();
		}

		public static string? CreateOrder(List<string> orderIds, List<OrderItem> items) {
			// Simulate a new order being created at the supplier with a 3 hour delivery time

			// Create fake supplier order
			string supplierOrderId = Guid.NewGuid().ToString().Substring(0, 6);

			// Fake expected delivery time, 24 hours
			TimeSpan expectedDeliveryTime = new TimeSpan(24, 0, 0);

			SupplierOrder newOrder = new SupplierOrder(supplierOrderId, DateTime.Now, 0, orderIds, items, expectedDeliveryTime);

			bool createResult = DataController.AddSupplierOrder(newOrder);

			if(!createResult) {
				return null;
			}

			return supplierOrderId;			
		}
	}
}