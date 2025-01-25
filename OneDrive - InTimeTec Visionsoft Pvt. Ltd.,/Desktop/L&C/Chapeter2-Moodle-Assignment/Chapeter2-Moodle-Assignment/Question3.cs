public class CustomerSearch
{
    public List<Customer> SearchByCountry(string country) => SearchCustomers(country, c => c.Country);
    public List<Customer> SearchByCompanyName(string company) => SearchCustomers(company, c => c.CompanyName);
    public List<Customer> SearchByContact(string contact) => SearchCustomers(contact, c => c.ContactName);

    private List<Customer> SearchCustomers(string filterKey, Func<Customer, string> filterProperty)
    {
        var query = from customer in db.customers
                    where filterProperty(customer).Contains(filterKey)
                    orderby customer.CustomerID ascending
                    select customer;
        return query.ToList();
    }
}

public class CustomerDataExporter
{
    public string GenerateCSV(List<Customer> customers) {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(var customer in customers)
        {
            stringBuilder.AppendFormat(GetCSVString(customer));
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }

    private static string GetCSVString(Customer customer)
    {
        return $"{customer.CustomerID},{customer.CompanyName},{customer.ContactName},{customer.Country}";
    }
}
