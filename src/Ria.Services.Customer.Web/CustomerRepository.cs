namespace Ria.Services.Customer.Web;

public class CustomerRepository
{
    public Customer[] Customers { get; private set; } = Array.Empty<Customer>();

    private HashSet<int> Ids { get; set; } = new HashSet<int>();

    public bool IdExists(Customer[] customers)
    {
        foreach (var customer in customers)
        {
            if (IdExists(customer.Id))
                return true;
        }

        return false;
    }

    public bool IdExists(int id) => Ids.Contains(id);

    public void AddCustomers(Customer[] customers)
    {
        // Sort logic implemented at Ria.Services.Customer.Web.Utilities extension method.
        customers.Sort(0, customers.Length - 1);

        var temp = new Customer[Customers.Length + customers.Length];

        var i = 0;
        var j = 0;
        var k = 0;

        while (i < Customers.Length && j < customers.Length)
        {
            if (Customers[i] < customers[j])
            {
                temp[k] = Customers[i];
                k++;
                i++;
            }
            else
            {
                temp[k] = customers[j];
                Ids.Add(customers[j].Id);
                k++;
                j++;
            }
        }

        while (i < Customers.Length)
        {
            temp[k] = Customers[i];
            k++;
            i++;
        }

        while (j < customers.Length)
        {
            temp[k] = customers[j];
            Ids.Add(customers[j].Id);
            k++;
            j++;
        }

        Customers = temp;
    }
}