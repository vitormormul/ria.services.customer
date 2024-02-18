namespace Ria.Services.Customer.Web;

public interface ICustomerRepository
{
    Customer[] Customers { get; }
    bool IdExists(Customer[] customers);
    bool IdExists(int id);
    Task AddCustomers(Customer[] customers);
}

public class CustomerRepository : ICustomerRepository
{
    private readonly ICustomerPublisher _customerPublisher;
    public Customer[] Customers { get; private set; } = Array.Empty<Customer>();
    private HashSet<int> Ids { get; } = new();

    public CustomerRepository(ICustomerPublisher customerPublisher)
    {
        _customerPublisher = customerPublisher;
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            using var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json"));
            var customers = reader.ReadToEnd();
            Customers = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer[]>(customers) ?? Array.Empty<Customer>();
            foreach (var customer in Customers)
            {
                Ids.Add(customer.Id);
            }
        }
        catch
        {
            Customers = Array.Empty<Customer>();
        }
    }

    public bool IdExists(Customer[] customers)
    {
        return customers.Any(customer => IdExists(customer.Id));
    }

    public bool IdExists(int id) => Ids.Contains(id);

    public async Task AddCustomers(Customer[] customers)
    {
        if (IdExists(customers))
        {
            throw new ArgumentException("Id already exists.");
        }
        
        // Sort logic implemented at Ria.Services.Customer.Web.Utilities extension method.
        customers.Sort();

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

        await _customerPublisher.WriteAsync(Customers);
    }
}