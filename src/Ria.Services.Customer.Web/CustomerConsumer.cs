using System.Threading.Channels;

namespace Ria.Services.Customer.Web;

public class CustomerConsumer : BackgroundService
{
    private readonly Channel<Customer[]> _customerChannel;
    private readonly ILogger<CustomerConsumer> _logger;

    public CustomerConsumer(Channel<Customer[]> customerChannel, ILogger<CustomerConsumer> logger)
    {
        _customerChannel = customerChannel;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var customers = await _customerChannel.Reader.ReadAsync(stoppingToken);
            await using var writer = new StreamWriter(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json"));
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(customers);
            await writer.WriteAsync(json);
            
            _logger.LogInformation($"Updated in-memory data with {customers.Length} records.");
            
            Thread.Sleep(1000);
        }
    }
}