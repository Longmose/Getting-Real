using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullyShipd
{
    public class Bestilling
    {

        public int Id { get; set; }                 // Bestillings-id
        public List<string> OrdreIdListe { get; set; } // Liste over ordre-ids
        public TimeSpan ForventetLeveringstid { get; set; } // Forventet leveringstid


        public Bestilling(int id, List<string> ordreIdListe, TimeSpan forventetLeveringstid)
        {
            Id = id;
            OrdreIdListe = ordreIdListe;
            ForventetLeveringstid = forventetLeveringstid;
        }

        // Metode til at tilføje en ordre-id til listen
        public void TilføjOrdreId(string ordreId)
        {
            OrdreIdListe.Add(ordreId);
        }
    }
}
