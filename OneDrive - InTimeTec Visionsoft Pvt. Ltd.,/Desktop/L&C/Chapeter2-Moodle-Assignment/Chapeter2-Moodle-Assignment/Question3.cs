public class CustomerSearch
{
    public List<Customer> SearchByCountry(string country){
        var query = from c in db.customers where c.Country.Contains(country) orderby c.CustomerID ascending select c;
        return query.ToList();
    }

    public List<Customer> SearchByCompanyName(string company) { 
        var query = from c in db.customers where c.Country.Contains(company) orderby c.CustomerID ascending select c;
        return query.ToList();
    }

    public List<Customer> SearchByContact(string contact) { 
        var query = from c in db.customers where c.Country.Contains(contact) orderby c.CustomerID ascending select c; 
        return query.ToList();
    }

    public string ConvertToCSV(List<Customer> customers) {
        StringBuilder stringBuilder = new StringBuilder();
        foreach(var customer in customers)
        {
            stringBuilder.AppendFormat("{0}, {1}, {2}, {3}", customer.CustomerID, customer.CompanyName, customer.ContactName, customer.Country);
            stringBuilder.AppendLine();
        }
        return stringBuilder.ToString();
    }
} 
