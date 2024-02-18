using System.Threading.Channels;

namespace Ria.Services.Customer.Web;

public interface ICustomerPublisher
{
    Task WriteAsync(Customer[] customers);
}

public class CustomerPublisher : ICustomerPublisher
{
    private readonly Channel<Customer[]> _customerChannel;
    public CustomerPublisher(Channel<Customer[]> customerChannel)
    {
        _customerChannel = customerChannel;
    }

    public async Task WriteAsync(Customer[] customers) => await _customerChannel.Writer.WriteAsync(customers);
}