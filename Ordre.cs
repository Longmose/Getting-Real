using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullyShipd
{

    public class Order
    {
        public int OrderId { get; set; }
        public List<Product> Products { get; set; }
        public string Status { get; set; }
        public string OrderPlacementDate { get; set; }
        public string OrderCompletionDate { get; set; }
        public string SupplierOrderId { get; set; }
        public bool ReadyForProduction { get; set; }

        public Order(int orderId, List<Product> products, string orderPlacementDate)
        {
            OrderId = orderId;
            Products = products;
            Status = "Afventer"; // Sætter ordrestatus automatisk til "Afventer", når den oprettes
            OrderPlacementDate = orderPlacementDate;
            ReadyForProduction = false; // Sætter standardværdi, om ordren er klar til produktionen
        }

        // Metode til at opdatere ordrens status
        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }

        // Metoden som markerer ordren, som klar til produktion
        public void MarkReadyForProduction()
        {
            ReadyForProduction = true;
        }

        // Metode til at opdatere færdiggørelsesdatoen for ordren
        public void UpdateCompletionDate(string completionDate)
        {
            OrderCompletionDate = completionDate;
        }

        // Metode til at opdatere leverandørens ordreID
        public void UpdateSupplierOrderId(string supplierOrderId)
        {
            SupplierOrderId = supplierOrderId;
        }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Price {  get; set; }

        //Man kan tilføje flere egenskaber her, ved ikke hvad der ellers mangler 
        public Product(int productId, string name, int quantity, double price)
        {
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            Price = price;
        }
    }
}
