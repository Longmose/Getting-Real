namespace FullyShipd
{
    internal class Program 
    {
        static void Main(string[] args)
        {
            Ordre ordre = new Ordre();

            // Opret en ny bestilling
            Bestilling bestilling1 = new Bestilling(1, new List<string>(), TimeSpan.FromDays(3));

            // Tilføj en ordre-id til bestillingen
            bestilling1.TilføjOrdreId("");

            // Udskriv bestillingsoplysninger
            Console.WriteLine("Bestillings-ID: " + bestilling1.Id);
            Console.WriteLine("Ordre-IDs: ");
            foreach (string ordreId in bestilling1.OrdreIdListe)
            {
                Console.WriteLine(ordreId);
            }
            Console.WriteLine("Forventet leveringstid: " + bestilling1.ForventetLeveringstid.TotalDays + " dage");
        }
    }
}
