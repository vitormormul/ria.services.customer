using Flurl.Http;
using Ria.Services.Customer.Web;


string[] firstNames = {
    "Leia",
    "Sadie",
    "Jose",
    "Sara",
    "Frank",
    "Dewey",
    "Tomas",
    "Joel",
    "Lukas",
    "Carlos"  
};

string[] lastNames = {
    "Liberty",
    "Ray",
    "Harrison",
    "Ronan",
    "Drew",
    "Powell",
    "Larsen",
    "Chan",
    "Anderson",
    "Lane"
};

var idRef = new Id();

var endpoint = Environment.GetEnvironmentVariable("CUSTOMER_API_ENDPOINT")
    ?? "http://localhost:5157/customers";

Console.WriteLine($"Sending requests to {endpoint}.");

while (true)
{
    var postTasks = new List<Task<IFlurlResponse>>();
    var getTasks = new List<Task<Customer[]>>();
    var random = new Random().Next(4, 10);
    var sum = 0;

    Parallel.ForEach(Enumerable.Range(0, random), (_, _) =>
    {
        var customers = GenerateCustomers();
        postTasks.Add(endpoint.PostJsonAsync(customers));
        getTasks.Add(endpoint.GetJsonAsync<Customer[]>());
        Interlocked.Add(ref sum, customers.Length);
    });
    
    Console.WriteLine($"Sent {random} new POST requests to the API with {sum} new customers.");
    Console.WriteLine($"Sent {random} new GET requests to the API.");

    Task.WhenAll(postTasks);
    Task.WhenAll(getTasks);
    Thread.Sleep(new Random().Next(100, 5000));
}

Customer[] GenerateCustomers()
{
    var customers = new Customer[new Random().Next(2, 8)];

    for (int i = 0; i < customers.Length; i++)
        customers[i] = GenerateCustomer();

    return customers;
}

Customer GenerateCustomer()
{
    var firstNameIndex = new Random().Next(firstNames.Length);
    var lastNameIndex = new Random().Next(lastNames.Length);
    var age = new Random().Next(10, 90);
    int id;

    lock (idRef!)
    {
        id = idRef.Value;
        idRef.Value++;
    }

    return new Customer
    {
        FirstName = firstNames[firstNameIndex],
        LastName = lastNames[lastNameIndex],
        Age = age,
        Id = id
    };
}

class Id
{
    public int Value;
}