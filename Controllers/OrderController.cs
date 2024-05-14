namespace FullyShipd {
	public static class OrderController {

		public static string FormatStatus(int status) {
			switch(status) {
				case 0: return "Pending";
				case 1: return "Awaiting supplier delivery";
				case 2: return "Ready for production";
				default: return "Unknown";
			}
		}

		public static List<Order> GetOrders() {
			// TODO: Handle if LoadOrders() returns null
			return DataController.LoadOrders() ?? new List<Order>();
		}

		public static bool SetOrderReadyForProduction(Order order) {
			// Check if order is in correct status
			if(order.Status != 0) {
				Console.WriteLine("Order is not in correct status to be set ready for production.");
				return false;
			}

			// Update order status
			order.Status = 2;

			// Update order in database
			bool updateResult = DataController.UpdateOrder(order);

			return updateResult;
		}

		public static void ProcessOrders(List<string> orderIds) {
			// Prepare variables
			List<Order> orders = GetOrders();
			List<Order> ordersWithoutDropshippedItems = new List<Order>();
			List<Order> ordersWithDropshippedItems = new List<Order>();
			Dictionary<string, int> orderQuantities = new Dictionary<string, int>();
			Dictionary<string, int> supplierOrderQuantities = new Dictionary<string, int>();

			// Loop through the selected order ids
			foreach(string orderId in orderIds) {
				Console.WriteLine($"Processing order {orderId}");
				var order = orders.Find(o => o.Id == orderId);

				if(order == null) {
					MessageBox.Show("An error occurred while processing the order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Check if the order contains dropshipped items
				bool hasDropshippedItems = order.Items.Any(i => InventoryController.GetInventoryItem(i.Sku)?.IsDropshipped == true);

				if(hasDropshippedItems) {
					ordersWithDropshippedItems.Add(order);
					foreach(OrderItem orderItem in order.Items) {
						var foundInventoryItem = InventoryController.GetInventoryItem(orderItem.Sku);
						if(foundInventoryItem == null) {
							MessageBox.Show($"Item {orderItem.Sku} not found in inventory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						// Add the quantity to the supplierOrderQuantities dictionary
						if(foundInventoryItem.IsDropshipped) {
							if(supplierOrderQuantities.ContainsKey(orderItem.Sku)) {
								supplierOrderQuantities[orderItem.Sku] += orderItem.Quantity;
							} else {
								supplierOrderQuantities[orderItem.Sku] = orderItem.Quantity;
							}
						}
					}
				} else {
					ordersWithoutDropshippedItems.Add(order);

					// Count the quantities of each item in the order and add them to the orderQuantities dictionary
					foreach(OrderItem orderItem in order.Items) {
						var foundInventoryItem = InventoryController.GetInventoryItem(orderItem.Sku);
						if(foundInventoryItem == null) {
							MessageBox.Show($"Item {orderItem.Sku} not found in inventory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}

						// Add the quantity to the orderQuantities dictionary
						if(orderQuantities.ContainsKey(orderItem.Sku)) {
							orderQuantities[orderItem.Sku] += orderItem.Quantity;
						} else {
							orderQuantities[orderItem.Sku] = orderItem.Quantity;
						}
					}
				}
			}

			// Check the requested items in ordersWithoutDropshippedItems against the inventory
			List<string> missingItems = new List<string>();
			bool requestedItemsInStock = orderQuantities.All(entry => {

				Console.WriteLine($"Checking stock for item {entry.Key}, requested {entry.Value}");
				
				var inventoryItem = InventoryController.GetInventoryItem(entry.Key);
				if(inventoryItem == null) {
					MessageBox.Show($"Item {entry.Key} not found in inventory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return false;
				}

				bool inStock = inventoryItem != null && inventoryItem.Stock >= entry.Value;

				if(!inStock) {
					missingItems.Add($"Requested {entry.Value} of {entry.Key}, but only {inventoryItem?.Stock} in stock.");
				}

				return inStock;				
			});

			// If any items are missing, show an error message and return
			if(!requestedItemsInStock) {
				MessageBox.Show($"The following items are missing from the inventory:\n{string.Join("\n", missingItems)}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// All the orders without dropshipped items can now be set to ready
			foreach(Order order in ordersWithoutDropshippedItems) {
				Console.WriteLine($"Order {order.Id} does not contain dropshipped items, setting to ready for production.");

				// Set the order status to ready
				bool updateResult = OrderController.SetOrderReadyForProduction(order);

				if(!updateResult) {
					MessageBox.Show("An error occurred while updating the order status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			if(ordersWithDropshippedItems.Count > 0) {
				Console.WriteLine("Creating supplier order for orders with dropshipped items.");

				List<string> dropshippedOrderIds = ordersWithDropshippedItems.Select(o => o.Id).ToList();

				// Convert supplierOrderQuantities to a list of OrderItems
				List<OrderItem> orderItems = supplierOrderQuantities.Select(entry => new OrderItem(entry.Key, entry.Value)).ToList();

				// Create supplier orders for the orders with dropshipped items
				var supplierOrderId = SupplierOrderController.CreateOrder(dropshippedOrderIds, orderItems);

				if(supplierOrderId == null) {
					MessageBox.Show("An error occurred while creating the supplier order.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				Console.WriteLine($"Supplier order created with ID {supplierOrderId}");

				// Set the status of the orders with dropshipped items to awaiting supplier delivery
				foreach(Order order in ordersWithDropshippedItems) {
					Console.WriteLine($"Setting order {order.Id} to awaiting supplier delivery.");

					// Set the order status to awaiting supplier delivery
					order.Status = 1;
					order.SupplierOrderId = supplierOrderId;

					// Update the order in the database
					bool updateResult = DataController.UpdateOrder(order);

					if(!updateResult) {
						MessageBox.Show("An error occurred while updating the order status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}

			// Show a message box to indicate that the orders have been processed
			MessageBox.Show("Selected orders have been processed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	}
}