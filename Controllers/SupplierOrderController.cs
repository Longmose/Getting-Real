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

		public static bool ArchiveOrder(SupplierOrder supplierOrder) {
			// Check if order is in correct status
			if(supplierOrder.Status != 1) {
				Console.WriteLine("Order is not in correct status to be archived.");
				return false;
			}

			// Update order status
			supplierOrder.Status = 2;

			// Update order in database
			bool updateResult = DataController.UpdateSupplierOrder(supplierOrder);

			return updateResult;
		}

		public static string? CreateOrder(List<string> orderIds, List<OrderItem> items) {
			// Simulate a new order being created at the supplier with a 3 hour delivery time

			// Create fake supplier order
			string supplierOrderId = Guid.NewGuid().ToString().Substring(0, 6);

			// Fake expected delivery time, 24 hours
			TimeSpan expectedDeliveryTime = new TimeSpan(24, 0, 0);
			
			// Fake set as ready for pickup
			int status = 1;

			SupplierOrder newOrder = new SupplierOrder(supplierOrderId, DateTime.Now, status, orderIds, items, expectedDeliveryTime);

			bool createResult = DataController.AddSupplierOrder(newOrder);

			if(!createResult) return null;

			return supplierOrderId;			
		}

		public static SupplierOrder? GetSupplierOrder(string supplierOrderId) {
			SupplierOrder? order = GetOrders().Find(o => o.Id == supplierOrderId);

			if(order == null) return null;

			return order;
		}
	}
}