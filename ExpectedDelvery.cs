namespace FullyShipd
{
    public class ExpectedDelvery
    {
        public string ForventetLevering(string leveringsland)
        {
            if (leveringsland == "Danmark")
            {
                return "Forventet levering inden for 3-5 arbjedsdage";
            }
            else
            {
                return "Forventet levering inden for 5-7 arbejdsdage";
            }
        }
    }

        
        



    
    
}
