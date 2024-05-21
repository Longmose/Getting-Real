namespace FullyShipd {
	static class Program {
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}

	class MainForm: Form {
		private TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
		private ListView pendingListView = new ListView();
		private ListView readyListView = new ListView();
		private ListView supplierOrdersView = new ListView();
		private ListView awaitingListView = new ListView();
		private Button reloadOrdersButton = new Button();
		private Button reloadSupplierOrdersButton = new Button();
		private Button processButton = new Button();
		private Button markDeliveredButton = new Button();

		public MainForm() {
			InitializeComponents();
			LoadOrders(); // Initial load of orders
			LoadSupplierOrders(); // Initial load of supplier orders
		}

		private void InitializeComponents() {
			Text = "FullyShipd Order Processing";
			ClientSize = new Size(1200, 450);

			// Setup TableLayoutPanel
			tableLayoutPanel.Dock = DockStyle.Fill;

			// Adding Column Sizes
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
			tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

			// Adding Row Sizes
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
			tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			Controls.Add(tableLayoutPanel);

			// Setup ListViews
			SetupListView(pendingListView, "Pending", true, false);
			SetupListView(readyListView, "Ready for production", false, false);
			SetupListView(supplierOrdersView, "Supplier orders", true, true);
			SetupListView(awaitingListView, "Awaiting Supplier", false, false);


			/* LEFT SIDE */
			tableLayoutPanel.Controls.Add(pendingListView, 0, 0);

			processButton.Text = "Process Selected Orders";
			processButton.Dock = DockStyle.Fill;
			processButton.Click += ProcessSelectedOrders;
			tableLayoutPanel.Controls.Add(processButton, 0, 1);

			tableLayoutPanel.Controls.Add(readyListView, 0, 2);

			// Reload orders Button
			reloadOrdersButton.Text = "Reload Orders";
			reloadOrdersButton.Dock = DockStyle.Fill;
			reloadOrdersButton.Click += (_, _) => LoadOrders();
			tableLayoutPanel.Controls.Add(reloadOrdersButton, 0, 3);

			/* RIGHT SIDE */
			tableLayoutPanel.Controls.Add(supplierOrdersView, 1, 0);

			markDeliveredButton.Text = "Set as delivered";
			markDeliveredButton.Dock = DockStyle.Fill;
			markDeliveredButton.Click += MarkDeliveredButtonSelectedOrders;
			tableLayoutPanel.Controls.Add(markDeliveredButton, 1, 1);

			tableLayoutPanel.Controls.Add(awaitingListView, 1, 2);

			// Reload supplier orders Button
			reloadSupplierOrdersButton.Text = "Reload Supplier Orders";
			reloadSupplierOrdersButton.Dock = DockStyle.Fill;
			reloadSupplierOrdersButton.Click += (_, _) => LoadSupplierOrders();
			tableLayoutPanel.Controls.Add(reloadSupplierOrdersButton, 1, 3);
		}

		private void SetupListView(ListView listView, string headerText, bool checkBoxes, bool isSupplierOrders) {
			listView.Dock = DockStyle.Fill;
			listView.View = View.Details;
			listView.CheckBoxes = checkBoxes;
			listView.Columns.Add(headerText, 130, HorizontalAlignment.Left);
			
			if(isSupplierOrders) {
				listView.Columns.Add("Date", 150, HorizontalAlignment.Left);
				listView.Columns.Add("Expected delivery", 150, HorizontalAlignment.Left);
			} else {
				listView.Columns.Add("Date", 150, HorizontalAlignment.Left);
			}

			listView.Columns.Add("Status", 150, HorizontalAlignment.Left);
			listView.FullRowSelect = false;
			listView.MultiSelect = false;
			listView.GridLines = true;
		}

		private void LoadOrders() {
			pendingListView.Items.Clear();
			awaitingListView.Items.Clear();
			readyListView.Items.Clear();

			List<Order> orders = OrderController.GetOrders();
			foreach(Order order in orders) {
				ListViewItem item = new ListViewItem(new[] {
					order.Id,
					order.Date.ToString("dd-MM-yyyy, HH:mm"),
					OrderController.FormatStatus(order.Status)
				}) {
					Checked = false
				};
				switch(order.Status) {
					case 0:
						pendingListView.Items.Add(item);
						break;
					case 1:
						awaitingListView.Items.Add(item);
						break;
					case 2:
						readyListView.Items.Add(item);
						break;
				}
			}
		}

		private void LoadSupplierOrders() {
			supplierOrdersView.Items.Clear();
			List<SupplierOrder> orders = SupplierOrderController.GetOrders();
			foreach(SupplierOrder order in orders) {

				// Calculate expexted delivery from order.date and order.ExpectedDeliveryTime
				var expectedDelivery = order.Date.Add(order.ExpectedDeliveryTime);

				ListViewItem item = new ListViewItem(new[] {
					order.Id,
					order.Date.ToString("dd-MM-yyyy, HH:mm"),
					expectedDelivery.ToString("dd-MM-yyyy, HH:mm"),
					SupplierOrderController.FormatStatus(order.Status)
				});

				supplierOrdersView.Items.Add(item);
			}
		}

		// Method to process selected orders
		private void ProcessSelectedOrders(object? sender, EventArgs e) {
			List<string> orderIds = pendingListView.CheckedItems.Cast<ListViewItem>().Select(item => item.Text).ToList();

			if(orderIds.Count == 0) {
				MessageBox.Show("Please select at least one order to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			OrderController.ProcessOrders(orderIds);

			// Reload the orders regardless of the result
			LoadOrders();
			LoadSupplierOrders();
		}

		// Method to mark selected supplier orders as delivered
		private void MarkDeliveredButtonSelectedOrders(object? sender, EventArgs e) {
			List<string> orderIds = supplierOrdersView.CheckedItems.Cast<ListViewItem>().Select(item => item.Text).ToList();

			if(orderIds.Count == 0) {
				MessageBox.Show("Please select at least one order to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			List<SupplierOrder> supplierOrders = new List<SupplierOrder>();
			List<Order> orders = new List<Order>();

			// Validate orders
			foreach(string orderId in orderIds) {
				SupplierOrder? order = SupplierOrderController.GetSupplierOrder(orderId);
				if(order == null) {
					MessageBox.Show($"Could not find supplier order with ID {orderId}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				if(order.Status != 1) {
					MessageBox.Show($"Supplier order with ID {orderId} is not ready for delivery.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}

				// Validate each order id in supplier order
				foreach(string id in order.OrderIds) {
					Order? orderItem = OrderController.GetOrder(id);
					if(orderItem == null) {
						MessageBox.Show($"Could not find order with ID {id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					if(orderItem.Status != 1) {
						MessageBox.Show($"Order with ID {id} is not awaiting supplier delivery.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					// Orders are valid, add to list
					orders.Add(orderItem);
				}

				// Orders are valid, add to list
				supplierOrders.Add(order);
			}

			// Mark orders as ready for production
			foreach(Order order in orders) {
				bool updateResult = OrderController.SetOrderReadyForProduction(order);

				if(!updateResult) {
					MessageBox.Show($"An error occurred while updating order with ID {order.Id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// Archive supplier orders
			foreach(SupplierOrder order in supplierOrders) {
				bool archiveResult = SupplierOrderController.ArchiveOrder(order);

				if(!archiveResult) {
					MessageBox.Show($"An error occurred while archiving supplier order with ID {order.Id}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			// Reload the orders
			LoadOrders();
			LoadSupplierOrders();
		}
	}
}
