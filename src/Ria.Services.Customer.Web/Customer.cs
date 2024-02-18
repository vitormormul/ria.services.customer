namespace Ria.Services.Customer.Web;

public class Customer
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public int Age { get; set; }
    public int Id { get; set; }

    public static bool operator <(Customer a, Customer b)
    {
        var lastNameComparison = String.Compare(a.LastName, b.LastName, StringComparison.Ordinal);

        if (lastNameComparison < 0) return true;
        if (lastNameComparison > 0) return false;
        
        var firstNameComparison = String.Compare(a.FirstName, b.FirstName, StringComparison.Ordinal);
        return firstNameComparison < 0;
    }

    public static bool operator >(Customer a, Customer b) => !(a < b);
    public static bool operator >=(Customer a, Customer b) => a > b;
    public static bool operator <=(Customer a, Customer b) => a < b;
}