using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullyShipd
{
    public class Product
    {
		private string productSKU;
        private string productSize;
		private int productWeight;
        private double productBuyPrice;

        public string ProductSKU
		{
			get { return productSKU; }
			set { productSKU = value; }
		}

		public string ProductSize
		{
			get { return productSize; }
			set { productSize = value; }
		}

		public double ProductBuyPrice
		{
			get { return productBuyPrice; }
			set { productBuyPrice = value; }
		}

		public int ProductWeight
		{
			get { return productWeight; }
			set { productWeight = value; }
		}
	}
}
