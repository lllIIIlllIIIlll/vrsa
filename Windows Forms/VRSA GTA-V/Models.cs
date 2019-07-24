using Newtonsoft.Json;
using System.Collections.Generic;

namespace Overlay.NET.GTAV.Models
{
    public class UserData
    {
        public BusinessesData Businesses = new BusinessesData();

        public User ToUser()
        {
            var result = new User();

            if (Businesses.Coke != null) result.Businesses.Add(new Business()
            {
                Name = "Coke",
                Sale = Businesses.Coke.Sale,
                Supply = Businesses.Coke.Supply
            });

            if (Businesses.Meth != null) result.Businesses.Add(new Business()
            {
                Name = "Meth",
                Sale = Businesses.Meth.Sale,
                Supply = Businesses.Meth.Supply
            });

            if (Businesses.CounterfeitCash != null) result.Businesses.Add(new Business()
            {
                Name = "Counterfeit Cash",
                Sale = Businesses.CounterfeitCash.Sale,
                Supply = Businesses.CounterfeitCash.Supply
            });

            if (Businesses.Nightclub != null) result.Businesses.Add(new Business()
            {
                Name = "Nightclub",
                Sale = Businesses.Nightclub.Sale,
                Supply = Businesses.Nightclub.Supply
            });

            return result;
        }
    }

    public class BusinessesData
    {
        [JsonProperty("Counterfeit Cash")]
        public BusinessData CounterfeitCash;
        public BusinessData Meth;
        public BusinessData Coke;
        public BusinessData Nightclub;
    }

    public class BusinessData
    {
        public decimal Sale { get; set; }
        public decimal Supply { get; set; }
    }

    public class User
    {
        public List<Business> Businesses = new List<Business>();
    }

    public class Business
    {
        public string Name { get; set; }
        public decimal Sale { get; set; }
        public decimal Supply { get; set; }
    }
}
