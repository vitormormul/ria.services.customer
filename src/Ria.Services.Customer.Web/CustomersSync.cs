using System.Threading.Channels;

namespace Ria.Services.Customer.Web;

public class CustomersSync : BackgroundService
{
    private readonly Channel<Customer[]> _customerChannel;

    public CustomersSync(Channel<Customer[]> customerChannel)
    {
        _customerChannel = customerChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var customers = await _customerChannel.Reader.ReadAsync(stoppingToken);
            await using var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json"));
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
            await writer.WriteAsync(json);
        }
    }
}