namespace Ria.Services.Customer.Web;

public class Customer
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public int Id { get; set; }

    public static bool operator <(Customer a, Customer b)
    {
        var lastNameComparison = String.Compare(a.LastName, b.LastName, StringComparison.Ordinal);

        if (lastNameComparison == -1) return true;
        if (lastNameComparison == 1) return false;
        
        var firstNameComparison = String.Compare(a.FirstName, b.FirstName, StringComparison.Ordinal);
        return firstNameComparison == -1;
    }

    public static bool operator >(Customer a, Customer b) => !(a < b);
    public static bool operator >=(Customer a, Customer b) => a > b;
    public static bool operator <=(Customer a, Customer b) => a < b;
}

public class CustomerRepository
{
    public Customer[] Customers { get; private set; } = Array.Empty<Customer>();

    public void AddCustomers(Customer[] customers)
    {
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
            k++;
            j++;
        }

        Customers = temp;
    }
}

public static class Utilities
{
    
    public static void Sort(this Customer[] arr, int left, int right)
    {
        if (left < right)
        {
            int pivot = Partition(arr, left, right);
 
            Sort(arr, left, pivot - 1);
            Sort(arr, pivot + 1, right);
        }
    }
 
    private static int Partition(Customer[] arr, int left, int right)
    {
        var pivot = arr[right];
        int i = left - 1;
 
        for (int j = left; j < right; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;
                var temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }
        }
 
        var temp1 = arr[i + 1];
        arr[i + 1] = arr[right];
        arr[right] = temp1;
 
        return i + 1;
    }
}