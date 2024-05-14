using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullyShipd
{
    public class Ordre
    {
        public string OrdreId { get; set; }
        public string SKU { get; set; }
        public int TotalPris { get; set; }
        public int LagerStatus { get; set; }
        public int OdreDato { get; set; }
        public string LeveringsLand { get; set; }
        public Ordre()
        {

        }
        public Ordre(string odreId, int totalpris, int lagerstatus, int odredato, string leveringsland, string SKU): this ()
        {
            OrdreId = odreId;
            TotalPris = totalpris;
            LagerStatus = lagerstatus;
            OdreDato = odredato;
            LeveringsLand = leveringsland;
            this.SKU = SKU;
        }
    
    }

        
        



    
    
}
