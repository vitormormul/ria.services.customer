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

var Id = new Id();

while (true)
{
    var tasks = new List<Task<IFlurlResponse>>();

    for (var i = 0; i < new Random().Next(2, 8); i++)
    {
        tasks.Add("http://localhost:5157/customers".PostJsonAsync(GenerateCustomers()));
    }

    Task.WhenAll(tasks);
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

    lock (Id!)
    {
        id = Id.Value;
        Id.Value++;
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