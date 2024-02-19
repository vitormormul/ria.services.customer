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
    private readonly ILogger<CustomerRepository> _logger;
    public Customer[] Customers { get; private set; } = Array.Empty<Customer>();
    private HashSet<int> Ids { get; } = new();

    public CustomerRepository(ICustomerPublisher customerPublisher, ILogger<CustomerRepository> logger)
    {
        _customerPublisher = customerPublisher;
        _logger = logger;
        LoadData();
    }

    private void LoadData()
    {
        lock (Customers)
        {
            try
            {
                using var reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json"));
                var customers = reader.ReadToEnd();
                Customers = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer[]>(customers) ??
                            Array.Empty<Customer>();
                foreach (var customer in Customers)
                {
                    Ids.Add(customer.Id);
                }

                _logger.LogInformation($"Loaded {Customers.Length} records from in-memory data.");
            }
            catch
            {
                Customers = Array.Empty<Customer>();
            }
        }
    }

    public bool IdExists(Customer[] customers)
    {
        var ids = customers.Select(x => x.Id);

        return customers.Any(customer => IdExists(customer.Id))
               && customers.Length == ids.Distinct().Count();
    }

    public bool IdExists(int id) => Ids.Contains(id);

    private void InsertCustomer(Customer customer)
    {
        if (Customers.Length == 0)
        {
            Customers = new[] { customer };
            Ids.Add(customer.Id);
            return;
        }

        var i = 0;
        var k = 0;

        var temp = new Customer[Customers.Length + 1];

        while (i < Customers.Length)
        {
            if (Customers[i] < customer)
            {
                temp[k] = Customers[i];
                k++;
                i++;
            }
            else
            {
                temp[k] = customer;
                Ids.Add(customer.Id);
                k++;
                break;
            }
        }

        if (i == Customers.Length)
        {
            temp[k] = customer;
            Ids.Add(customer.Id);
        }

        while (i < Customers.Length)
        {
            temp[k] = Customers[i];
            k++;
            i++;
        }

        Customers = temp;
    }

    public async Task AddCustomers(Customer[] customers)
    {
        if (IdExists(customers))
        {
            throw new ArgumentException("Id already exists.");
        }

        lock (Customers)
        {
            foreach (var customer in customers)
                InsertCustomer(customer);
        }

        await _customerPublisher.WriteAsync(Customers);
    }
}