using System;

namespace CountryCodeApp
{
    class Program
    {
        struct Country(string code, string fullName)
        {
            public string Code { get; set; } = code;
            public string FullName { get; set; } = fullName;
        }

        static string GetCountryFullName(string countryCode, Country[] countries)
        {
            foreach (var country in countries)
            {
                if (country.Code.ToLower() == countryCode.ToLower())
                {
                    return $"Full Country Name: {country.FullName}";
                }
            }
            return "Country code not found";
        }

        static void Main(string[] args)
        {
            Country[] countries = {
                new ("IN", "India"),
                new ("US", "United States"),
                new ("NZ", "New Zealand"),
                new ("CA", "Canada"),
                new ("AU", "Australia")
            };

            Console.Write("Enter a country code (e.g., IN, US, NZ, CA, AU): ");
            string countryCode = Console.ReadLine()??"";

            if(countryCode == null)
            {
                return;
            }
            string fullName = GetCountryFullName(countryCode, countries);
            Console.WriteLine(fullName);
        }
    }
}
